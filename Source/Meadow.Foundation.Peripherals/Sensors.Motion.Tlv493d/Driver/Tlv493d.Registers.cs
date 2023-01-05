namespace Meadow.Foundation.Sensors.Motion
{
    public partial class Tlv493d
    {
        /// <summary>
        /// Register addresses for the Tlv493d
        /// </summary>
        static class Registers
        {
			//read registers
			public const byte Bx = 0x00;
			public const byte By = 0x01;
			public const byte Bz = 0x02;
			public const byte Temp = 0x03;
			public const byte Bx2 = 0x04;
            public const byte Bz2 = 0x05;
            public const byte Temp2 = 0x06;
			public const byte FactSet1 = 0x07;
            public const byte FactSet2 = 0x08;
            public const byte FactSet3 = 0x09;


			//write registers
			public const byte Res0 = 0x00;
            public const byte Mode1 = 0x01;
			public const byte Res2 = 0x02;
            public const byte Mode2 = 0x03;


            /*
			public const byte R_BX1 = 0;
			public const byte R_BX2 = 1;
			public const byte R_BY1 = 2;
			public const byte R_BY2 = 3;
			public const byte R_BZ1 = 4;
			public const byte R_BZ2 = 5;
			public const byte R_TEMP1 = 6;
			public const byte R_TEMP2 = 7;
			public const byte R_FRAMECOUNTER = 8;
			public const byte R_CHANNEL = 9;
			public const byte R_POWERDOWNFLAG = 10;
			public const byte R_RES1 = 11;
			public const byte R_RES2 = 12;
			public const byte R_RES3 = 13;

			public const byte W_PARITY = 14;
			public const byte W_ADDR = 15;
			public const byte W_INT = 16;
			public const byte W_FAST = 17;
			public const byte W_LOWPOWER = 18;
			public const byte W_TEMP_NEN = 19;
			public const byte W_LP_PERIOD = 20;
			public const byte W_PARITY_EN = 21;
			public const byte W_RES1 = 22;
			public const byte W_RES2 = 23;
			public const byte W_RESS = 24; */
        }
    }
}