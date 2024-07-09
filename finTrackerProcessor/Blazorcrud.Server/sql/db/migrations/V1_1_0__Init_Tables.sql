create table if not exists Users(
    Id           INTEGER not null
    constraint PK_Users
    primary key autoincrement,
    FirstName    TEXT    not null,
    LastName     TEXT    not null,
    Username     TEXT    not null,
    Password     TEXT    not null,
    Token        TEXT,
    IsDeleting   INTEGER not null,
    PasswordHash TEXT
);

CREATE TABLE if not exists Categories(
     Id               INTEGER not null
     constraint PK_Categories
     primary key autoincrement,
     ParentCategoryId INTEGER
     constraint FK_Categories_Categories_ParentCategoryId
     references Categories,
     "Order"          INTEGER not null,
     Title            TEXT    not null,
     PrefixedTitle    TEXT    not null,
     Colour           TEXT    not null,
     IsBill           INTEGER not null,
     IsTransfer       INTEGER not null,
     DateCreated      TEXT    not null DEFAULT CURRENT_TIMESTAMP,
     DateDeleted      TEXT    null
);

create index IX_Categories_ParentCategoryId
    on Categories (ParentCategoryId);

---------------------------------------------------------------------------------------------------------

create table if not exists MerchantKeywords
(
    Id               INTEGER not null
        constraint PK_MerchantKeywords
        primary key autoincrement,
    Keyword     TEXT    not null,
    DateCreated TEXT    not null DEFAULT CURRENT_TIMESTAMP,
    DateDeleted TEXT    null
);

---------------------------------------------------------------------------------------------------------

CREATE TABLE if not exists MerchantKeywordsCategories
(
    Id INTEGER not null
    constraint PK_MerchantKeywordsCategories
    primary key autoincrement,
    KeywordId   INTEGER not null
    constraint FK_MerchantKeywordsCategories_MerchantKeywords_KeywordId
    references MerchantKeywords
    on delete cascade,
    CategoryId  INTEGER not null
    constraint FK_MerchantKeywordsCategories_Categories_CategoryId
    references Categories
    on delete cascade,
    IsDisabled  INTEGER     not null,
    DateCreated TEXT    not null DEFAULT CURRENT_TIMESTAMP,
    DateDeleted TEXT    null
);

create index IX_MerchantKeywordsCategories_CategoryId
    on MerchantKeywordsCategories (CategoryId);

create index IX_MerchantKeywordsCategories_KeywordId
    on MerchantKeywordsCategories (KeywordId);


CREATE TABLE if not exists MerchantKeywordsCategories_Temp (
    Id INTEGER not null
        constraint PK_MerchantKeywordsCategoriesTemp
        primary key autoincrement,
    Keyword TEXT    not null,
    Title   TEXT    not null
);


