USE HappyCaroDB;
GO

SET DATEFORMAT DMY
-- ======================================
-- 1. USERS
-- Cập nhật: Thêm cột Draws
-- ======================================
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    RankPoint INT DEFAULT 1000,
    Wins INT DEFAULT 0,
    Losses INT DEFAULT 0,
    Draws INT DEFAULT 0, -- Bổ sung: Số trận hòa
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- ======================================
-- 2. FRIENDS
-- Cập nhật: Thêm UNIQUE Constraint để đảm bảo mỗi cặp chỉ có 1 bản ghi
-- ======================================
CREATE TABLE Friends (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    FriendId INT NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'PENDING',
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (FriendId) REFERENCES Users(Id),
);
GO

CREATE INDEX IDX_Friends_User ON Friends(UserId);
CREATE INDEX IDX_Friends_Friend ON Friends(FriendId);
GO

-- ======================================
-- 3. ROOMS
-- Cập nhật: Thêm IsPrivate và Password cho phòng riêng
-- ======================================
CREATE TABLE Rooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomName NVARCHAR(100) NOT NULL,
    HostId INT NOT NULL,
    IsPrivate BIT NOT NULL DEFAULT 0, -- Bổ sung: Phòng có phải là riêng tư không
    Password NVARCHAR(50) NULL,       -- Bổ sung: Mật khẩu (chỉ dùng nếu IsPrivate=1)
    Status INT NOT NULL DEFAULT 0,   -- 0=waiting, 1=playing, 2=closed
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (HostId) REFERENCES Users(Id)
);
GO

-- ======================================
-- 4. MATCHES
-- Cập nhật: Thêm EndReason để ghi lại lý do kết thúc trận đấu
-- ======================================
CREATE TABLE Matches (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomId INT NOT NULL,
    PlayerXId INT NOT NULL,
    PlayerOId INT NOT NULL,
    WinnerId INT NULL,
    EndReason NVARCHAR(50) NULL, -- Bổ sung: Lý do kết thúc (WIN, DRAW, SURRENDER, DISCONNECT)
    StartedAt DATETIME DEFAULT GETDATE(),
    EndedAt DATETIME NULL,

    FOREIGN KEY (RoomId) REFERENCES Rooms(Id),
    FOREIGN KEY (PlayerXId) REFERENCES Users(Id),
    FOREIGN KEY (PlayerOId) REFERENCES Users(Id),
    FOREIGN KEY (WinnerId) REFERENCES Users(Id)
);
GO

-- ======================================
-- 5. MOVES (từng nước đi)
-- Cập nhật: Thêm UNIQUE Constraint trên MatchId và StepNumber
-- ======================================
CREATE TABLE Moves (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MatchId INT NOT NULL,
    PlayerId INT NOT NULL,
    X INT NOT NULL,
    Y INT NOT NULL,
    StepNumber INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (MatchId) REFERENCES Matches(Id),
    FOREIGN KEY (PlayerId) REFERENCES Users(Id),
    
    CONSTRAINT UQ_Moves_Sequence UNIQUE (MatchId, StepNumber) -- Đảm bảo thứ tự nước đi là duy nhất
);

-- ======================================
-- 6. MESSAGES (chat)
-- Cập nhật: Thêm CHECK Constraint
-- ======================================
CREATE TABLE Messages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SenderId INT NOT NULL,
    RoomId INT NULL,
    MatchId INT NULL,
    Content NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id),
    FOREIGN KEY (MatchId) REFERENCES Matches(Id),
    
    CONSTRAINT CK_Messages_Context CHECK (RoomId IS NOT NULL OR MatchId IS NOT NULL) -- Đảm bảo tin nhắn thuộc ít nhất 1 ngữ cảnh (Room hoặc Match)
);
GO

CREATE TABLE ResetTokens (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Token NVARCHAR(255) NOT NULL, -- Để lưu mã code 6 số hoặc chuỗi GUID
    ExpiryTime DATETIME NOT NULL, -- Thời gian mã hết hạn
    Used BIT DEFAULT 0, -- Cờ để đánh dấu mã đã được sử dụng
    
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO