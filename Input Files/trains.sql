
SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

CREATE TABLE IF NOT EXISTS `trains` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `train_number` varchar(10) NOT NULL COMMENT 'номер потяга',
  `destination` varchar(50) NOT NULL COMMENT 'станція призначення',
  `departure_time` time NOT NULL COMMENT 'час відправлення',
  `travel_time` time NOT NULL COMMENT 'час у дорозі',
  `tickets_available` int(4) NOT NULL COMMENT 'наявність квитків',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=11 ;

INSERT INTO `trains` (`id`, `train_number`, `destination`, `departure_time`, `travel_time`, `tickets_available`) VALUES
(1, '001К', 'Київ', '08:30:00', '05:20:00', 120),
(2, '045Л', 'Львів', '12:15:00', '07:45:00', 80),
(3, '102О', 'Одеса', '06:50:00', '06:10:00', 50),
(4, '215Х', 'Харків', '14:40:00', '04:30:00', 60),
(5, '300З', 'Запоріжжя', '09:00:00', '06:50:00', 40),
(6, '411Ч', 'Чернігів', '17:20:00', '03:15:00', 35),
(7, '522М', 'Миколаїв', '11:10:00', '08:00:00', 70),
(8, '633В', 'Вінниця', '15:45:00', '02:40:00', 25),
(9, '744Т', 'Тернопіль', '19:30:00', '04:55:00', 90),
(10, '855У', 'Ужгород', '21:00:00', '10:30:00', 30);
