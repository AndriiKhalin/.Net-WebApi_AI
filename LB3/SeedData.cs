﻿namespace LB3;

public static class SeedData
{
    public static void Seed(ComputerDbContext context)
    {
        if (!context.HardDrives.Any())
        {
            var intel1 = new Processor()
            {
                Name = "Intel i5 7300",
                ClockFrequency = 3200,
                SocketType = "socket",
                NumCores = 6,
                NumThreads = 12,
                Price = 10000,
                ProcessorFamily = "i5",
                ProcessorGeneration = "socket 1700"
            };
            var intel2 = new Processor()
            {
                Name = "Intel i5 8400",
                ClockFrequency = 4200,
                NumCores = 6,
                SocketType = "socket",
                NumThreads = 12,
                Price = 1000,
                ProcessorFamily = "i5",
                ProcessorGeneration = "socket 1800"
            };
            context.Processors.AddRangeAsync(intel1, intel2);


            var nvidia = new VideoCard()
            {
                Name = "GTX 1650",
                ClockFrequency = 1500,
                SeriesName = 16,
                VideoMemory = 4096,
                Price = 10000
            };

            var nvidia1 = new VideoCard()
            {
                Name = "RTX 3050",
                ClockFrequency = 1700,
                SeriesName = 30,
                VideoMemory = 4096,
                Price = 13000
            };

            context.VideoCards.AddRangeAsync(nvidia, nvidia1);

            var hdd = new HardDrive()
            {
                Name = "Western Digitle",
                HardDriveType = "HDD",
                StorageSize = 1000,
                ReadWriteSpeed = 150,
                Price = 1000
            };
            var sdd = new HardDrive()
            {
                Name = "Kingston",
                HardDriveType = "SSD",
                StorageSize = 1000,
                ReadWriteSpeed = 550,
                Price = 2000
            };
            context.HardDrives.AddRangeAsync(hdd, sdd);

            var ram1 = new Ram()
            {
                Name = "Kingston Fury",
                TypeMemory = "DDR4",
                Size = 16385,
                OperatingFrequency = 3200,
                Price = 3000
            };
            var ram2 = new Ram()
            {
                Name = "Micron",
                TypeMemory = "DDR3",
                Size = 16385,
                OperatingFrequency = 2666,
                Price = 2000
            };
            context.Rams.AddRangeAsync(ram1, ram2);

            var mother1 = new MotherBoard()
            {
                Name = "Asus",
                Chipset = "mxx",
                FormFactor = "atx",
                Socket = "1700",
                Price = 2000
            };
            var mother2 = new MotherBoard()
            {
                Name = "Gigybite",
                Chipset = "mxx",
                FormFactor = "atx",
                Socket = "amd3",
                Price = 3000
            };
            context.MotherBoards.AddRangeAsync(mother1, mother2);

            var unit1 = new Unit()
            {
                Name = "Article",
                FormFactorMotherBoard = "mxx",
                Height = 200,
                Length = 300,
                Width = 150,
                Price = 2400
            };
            var unit2 = new Unit()
            {
                Name = "Article",
                FormFactorMotherBoard = "mxx",
                Height = 200,
                Length = 300,
                Width = 150,
                Price = 2400
            };
            context.Units.AddRangeAsync(unit1, unit2);

            context.SaveChanges();
        }

    }
}