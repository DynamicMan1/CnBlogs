/*
Navicat MySQL Data Transfer

Source Server         : localdb
Source Server Version : 50714
Source Host           : localhost:3306
Source Database       : cnblogsdb

Target Server Type    : MYSQL
Target Server Version : 50714
File Encoding         : 65001

Date: 2017-04-05 11:45:48
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for blogapply
-- ----------------------------
DROP TABLE IF EXISTS `blogapply`;
CREATE TABLE `blogapply` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `Reason` varchar(4000) NOT NULL,
  `RealName` varchar(255) DEFAULT NULL,
  `Position` varchar(255) DEFAULT NULL,
  `Unit` varchar(255) DEFAULT NULL,
  `Interest` varchar(255) DEFAULT NULL,
  `IsRead` tinyint(1) NOT NULL DEFAULT '0',
  `LastModifiedTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_user` (`UserId`),
  CONSTRAINT `fk_user` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for email
-- ----------------------------
DROP TABLE IF EXISTS `email`;
CREATE TABLE `email` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `PrivateKeyJson` varchar(900) NOT NULL,
  `PublicKeyJson` varchar(900) NOT NULL,
  `ActionType` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Email` varchar(64) NOT NULL,
  `UserName` varchar(64) NOT NULL,
  `DisplayName` varchar(20) NOT NULL,
  `Password` varchar(64) NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastModifiedTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastLoginTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LoginTimes` int(11) NOT NULL,
  `IsActivate` tinyint(1) NOT NULL DEFAULT '0',
  `IsBlogApply` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uni_Email` (`Email`),
  UNIQUE KEY `uni_Display` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS=1;
