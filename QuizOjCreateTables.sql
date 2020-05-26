Create Schema QOJ;
use QOJ;

drop table `UserInfo`;
drop table `OJ`;
drop table `Quiz`;
drop table `QuizOption`;
drop table `OjSuccess`;
drop table `OjSuccessCount`;
drop table `QuizHighScore`;
drop table `OjTestCase`;

-- 1
Create Table `UserInfo`(
	`id` VARCHAR(35) NOT NULL,
	`name` VARCHAR(20) Unique NOT NULL,
    `pwd` VARCHAR(16) NOT NULL,
    primary key (`id`),
    index nameIndex (`name`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 2
Create Table `OJ`(
	`id` VARCHAR(35) NOT NULL,
    `title` VARCHAR(20) UNIQUE NOT NULL,
    `content` VARCHAR(500) NOT NULL,
    `code` Text,
    `difficulty` INT NOT NULL,
    `createTime` Datetime NOT NULL,
    primary key (`id`),
    INDEX titleIndex (`title`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


-- 3
Create Table `Quiz`(
	`id` VARCHAR(35) NOT NULL,
    `description` VARCHAR(200) NOT NULL,
    primary key (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


-- 4
Create Table `QuizOption`(
	`quizId` VARCHAR(35) NOT NULL references `Quiz`(`id`),
    `optionId` INT unsigned NOT NULL,
    `description` VARCHAR(50) NOT NULL,
    `correct` bool NOT NULL,
    primary key (`quizId`, `optionId`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


-- 5
Create Table `OjSuccess`(
	`userId` VARCHAR(35) NOT NULL references `UserInfo`(`id`),
    `ojId` VARCHAR(35) NOT NULL references `OJ`(`id`),
    primary key(`userId`, `ojId`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


-- 6
Create Table `OjSuccessCount`(
	`userId` VARCHAR(35) NOT NULL references `UserInfo`(`id`),
    `successOjCount` INT unsigned NOT NULL,
    primary key(`userId`),
    Index successOjCountIndex (`successOjCount`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


-- 7
Create Table `QuizHighScore`(
	`userId` VARCHAR(35) NOT NULL references `UserInfo`(`id`),
    `highScore` Int unsigned NOT NULL,
    primary key (`userId`),
    Index highScoreIndex (`highScore`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


-- 8
Create Table `OjTestCase`(
	`ojId` VARCHAR(35) NOT NULL references `OJ`(`id`),
    `testCaseJson` Text,
    primary key (`ojId`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;