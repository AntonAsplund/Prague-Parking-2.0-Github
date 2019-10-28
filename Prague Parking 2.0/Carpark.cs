using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Threading;

namespace Prague_Parking_2._0
{
    class Carpark
    {
        static void Main(string[] args)
        {
            //Till Paul,Claes eller den som rättar.
            //
            //Programmet kan hantera upp till 200 objekt(pga två mc i en parkeringsplats) av klassen Vehicle som har propertyna  "Typ av fordon, Reg plåt, Ankomst tid".
            //Alla alfabet som stöds av UNICODE kan hanteras av programmet men typsnittet i konsol fönstret begränsar vad som kan visas. NSinSam är det typsnittet som jag upplevt kan visa mest.
            //
            //Val #7 i menyn leder till optimerings funktionerna. Där produceras en "Workorder Optimization.txt" som sparas sålänge programmet är igång. Den slängs automatiskt om programmet stängs av korrekt.
            //Hela filen skrivs också ut i konosl fönstret efter varje gång en optimering gjorts. Men med tydlig linje var en ny optimering påbörjas samt en tidstämpel. 
            //Allt för att evetuell felhantering av kund inte ska leda till dataförlust
            //
            //Bool TESTARRAY - rad 77 kan sättas i "false" läge för att det inte ska läggas till test fordon. Den står nu i "true" läge som lägger till nya fordon varje gång programmet startas.
            //
            //När kunden hämtar ut ett fordon så produceras ett kvitto till kunden som skriv i konsolfönsret med "Regnr, parkerade timmar och minuter, total konstnad" samt 
            //ett kvitto till personal som det står vilken parkering fordonet står på och vart de ska köras till. Antingen till en annan parkering eller till kunden.
            //
            //I övrigt är programmet framtaget helt enligt uppgiften för VG och G nivå med fokus på att inte förlora data, användarvänligheten och tidsbesparing för kunden. 
            //
            //
            //  Återkom gärna med frågor om det är nått! Jag har//Anton Asplund - SUT19

            Console.OutputEncoding = Encoding.Unicode;          //Fixar så att programmet kan läsa och skriva ut inte latinska bokstäver typ kyrrilliska eller japanska. 
            Console.InputEncoding = Encoding.Unicode;           //Men NSinSam typsnitt i konsolfönsret bör sättas så att sjävla konsolfönsret kan läsa tecknen/bokstäverna.

            VehicleTypeEnum Car = VehicleTypeEnum.Car;      //ENUM för bil
            VehicleTypeEnum MC = VehicleTypeEnum.MC;        //ENUM för MC

            Presentasciiartfirsttime();

            Console.WriteLine("Welcome to Parking Program Prague 2.0");
            Console.WriteLine("© Anton Asplund-SUT19");
            Console.WriteLine("");
            Returntomenu();



            bool switchCaseLoop = true;     //Håller kvar användaren i huvudemenyn till dess att exit valet görs och programmet avslutas

            Vehicle[,] vehicleArray = new Vehicle[100, 2]; //Skapar en array med 100 platser, en parkeringsplats skall kunna innehålla 2 st motorcyklar också. Därav den mulitdimensionella arrayn.

            int lineTracker = 0;    //Läser in hela "Parking array save file.txt" som hämtar all info från spar filen.
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vehicle temp = null;

                    temp = Getinfofromsavefile(lineTracker);


                    if (temp != null)
                    {
                        vehicleArray[i, j] = temp;
                    }

                    if (temp == null)
                    {
                        vehicleArray[i, j] = null;
                    }

                    lineTracker = lineTracker + 3;


                }
            }           

            bool TESTTHEARRAY = true; // Testar att lägga till några fordon på parkeringen. true=Lägga till fordonen. false=icke lägga till testfordon igen. OBS så hämtas data in från sparfil tidigare, och den datan riskeras då att skrivas över
            if (TESTTHEARRAY)
            {
                

                DateTime parkingTimeTestArray = DateTime.Now;
                DateTime parkingTimeTestArrayOld1 = Convert.ToDateTime("2019 - 09 - 18 09:15:52");
                DateTime parkingTimeTestArrayOld2 = Convert.ToDateTime("2019 - 10 - 08 09:15:52");
                DateTime parkingTimeTestArrayOld3 = Convert.ToDateTime("2019 - 09 - 08 09:15:52");

                Vehicle TestArray1 = new Vehicle(MC, "プログラマー", parkingTimeTestArray);
                Vehicle TestArray2 = new Vehicle(MC, "драсти", parkingTimeTestArray);
                Vehicle TestArray3 = new Vehicle(MC, "ORU271", parkingTimeTestArray);
                Vehicle TestArray4 = new Vehicle(MC, "ORU270", parkingTimeTestArrayOld1);
                Vehicle TestArray5 = new Vehicle(MC, "โปรแกรมเมอร์", parkingTimeTestArray);
                Vehicle TestArray6 = new Vehicle(MC, "UZU840", parkingTimeTestArrayOld2);
                Vehicle TestArray7 = new Vehicle(MC, "FCJ355", parkingTimeTestArray);
                Vehicle TestArray8 = new Vehicle(Car, "LZC418", parkingTimeTestArray);
                Vehicle TestArray9 = new Vehicle(Car, "DÜ59868", parkingTimeTestArrayOld3);
                Vehicle TestArray10 = new Vehicle(Car, "8554456", parkingTimeTestArray);

                vehicleArray[13, 1] = TestArray1;
                vehicleArray[62, 0] = TestArray2;
                vehicleArray[34, 0] = TestArray3;
                vehicleArray[13, 0] = TestArray4;
                vehicleArray[54, 0] = TestArray5;
                vehicleArray[78, 1] = TestArray6;
                vehicleArray[98, 0] = TestArray7;
                vehicleArray[57, 0] = TestArray8;
                vehicleArray[67, 0] = TestArray9;
                vehicleArray[43, 0] = TestArray10;

            }

            


            int switchCaseMenu = 0;

            do
            {

                switchCaseMenu = Presentswitchcasemenu();

                switch (switchCaseMenu)
                {
                    case 1:             //Lägger till ett fordon på parkeringen
                        {
                            VehicleTypeEnum tempAddVehicle = Getvehicletype();

                            Console.Write("Enter vehicle licensenumber: ");
                            string tempVehicleLicense = Getlicensenumber();

                            DateTime tempParkingTimeNow = DateTime.Now;

                            bool vehicleAdded = false;

                            Vehicle tempVehicle = new Vehicle(tempAddVehicle, tempVehicleLicense, tempParkingTimeNow);

                            if (tempAddVehicle == VehicleTypeEnum.Car)  //Kollar efter första lediga platsen för en bil i parkeringen
                            {
                                for (int i = 0; i < 100; i++)
                                {
                                    if (vehicleArray[i, 0] == null && vehicleArray[i, 1] == null)
                                    {
                                        vehicleArray[i, 0] = tempVehicle;
                                        Console.WriteLine("The {0} has been added to parking space {1}.", tempAddVehicle, (i + 1));
                                        vehicleAdded = true;
                                        i = 100;
                                    }
                                }
                            }

                            if (tempAddVehicle == VehicleTypeEnum.MC)  //Kollar efter första lediga plats i parkeringen om fordonet är en MC
                            {
                                for (int i = 0; i < 100; i++)
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        if (vehicleArray[i, 0] == null)                     //Kollar om första platsen i en parkering är 
                                        {
                                            vehicleArray[i, 0] = tempVehicle;
                                            Console.WriteLine("The {0} has been added to parking space {1}.", tempAddVehicle, (i + 1));
                                            vehicleAdded = true;
                                        }

                                        if (vehicleAdded == false)
                                        {
                                            if (vehicleArray[i, 0].VehicleTypes == MC && vehicleArray[i, 1] == null)
                                            {
                                                vehicleArray[i, 1] = tempVehicle;
                                                Console.WriteLine("The {0} has been added to parking space {1}.", tempAddVehicle, (i + 1));
                                                vehicleAdded = true;
                                            }
                                        }

                                        if (vehicleAdded)
                                        {
                                            i = 100;
                                            j = 2;
                                        }
                                    }
                                }
                            }

                            if (!vehicleAdded)
                            {
                                Console.WriteLine("The parking space is full by accordance to this specific type of vehicle, thus no vehicle has been added to the parking lot.");
                            }

                            for (int i = 0; i < 100; i++)       //Skriver ut arrayn till en textfil som sparas "Parking array save file.txt"
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    Vehicle saveTempVehicle = vehicleArray[i, j];
                                    Writetotempfile(saveTempVehicle);
                                }
                            }

                            Writetooriginalfile();


                            Returntomenu();
                            break;
                        }
                    case 2:             //Manuell flytt av fordon ifrån en sagd parkering till en annan
                        {
                            Console.WriteLine("Which vehicle do you want to move?");
                            Console.Write("Please enter licensenumber: ");

                            string licenseNumber = Getlicensenumber();
                            int indexOfSearchedPlateI = 0;
                            int indexOfSearchedPlateJ = 0;
                            string licensePlateFound = "";

                            for (int i = 0; i < 100; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (vehicleArray[i, j] != null)
                                    {
                                        string tempLicensePlate = vehicleArray[i, j].LicensePlate;

                                        if (tempLicensePlate.Contains(licenseNumber) == true)
                                        {
                                            indexOfSearchedPlateI = i;
                                            indexOfSearchedPlateJ = j;
                                            licensePlateFound = tempLicensePlate;
                                        }
                                    }
                                }
                            }

                            if (licensePlateFound != "")
                            {
                                Console.WriteLine("Is {0} the licensenumber of the vehicle you want to move?", licensePlateFound);
                                Console.WriteLine("1. Yes");
                                Console.WriteLine("2. No");

                                int numInputMoveVehicle = Inputfailsafetwo();

                                if (numInputMoveVehicle == 1)
                                {

                                    Vehicle tempVehicle = vehicleArray[indexOfSearchedPlateI, indexOfSearchedPlateJ];

                                    Console.WriteLine("Where Do you want to move the vehicle?");
                                    Console.Write("Enter the desired parking space: ");
                                    int parkingSpaceNumber = Inputfailsafeonehundred();
                                    parkingSpaceNumber = parkingSpaceNumber - 1;            //Anpassa numret till en indexpostition


                                    if (tempVehicle.VehicleTypes == MC)
                                    {
                                        if (vehicleArray[parkingSpaceNumber, 0] != null && vehicleArray[parkingSpaceNumber, 1] != null) //Lägger inte till något i rutan då parkeringsplatsen är upptagen av två mc's
                                        {
                                            Console.WriteLine("The parking space is occupied by two MC's. Therefore no MC has been moved");
                                        }

                                        if (vehicleArray[parkingSpaceNumber, 0] != null && vehicleArray[parkingSpaceNumber, 1] == null) //Lägger till motorcykeln på den andra platsen i rutan
                                        {
                                            Vehicle tempCarMC = vehicleArray[parkingSpaceNumber, 0];

                                            if (tempCarMC.VehicleTypes == VehicleTypeEnum.MC)           //Om den första platsen i multi arrayn är en MC
                                            {
                                                Console.WriteLine("The MC has been added to the parkingspace together with another MC");
                                                vehicleArray[parkingSpaceNumber, 1] = tempVehicle;
                                                vehicleArray[indexOfSearchedPlateI, indexOfSearchedPlateJ] = null;
                                                Staffreceiptmovevehicle(tempVehicle.LicensePlate, (indexOfSearchedPlateI + 1), (parkingSpaceNumber + 1));
                                            }

                                            if (tempCarMC.VehicleTypes == VehicleTypeEnum.Car)          //om den första platsen är upptagen av en bil
                                            {
                                                Console.WriteLine("The parking space is occupied by a car. Therefore no MC has been moved.");
                                            }
                                        }

                                        if (vehicleArray[parkingSpaceNumber, 0] == null && vehicleArray[parkingSpaceNumber, 1] != null) //Lägger till motorcykeln på den första platsen i parkeringsrutan
                                        {
                                            Console.WriteLine("The MC has been added to the parkingspace together with another MC");
                                            vehicleArray[parkingSpaceNumber, 0] = tempVehicle;
                                            vehicleArray[indexOfSearchedPlateI, indexOfSearchedPlateJ] = null;
                                            Staffreceiptmovevehicle(tempVehicle.LicensePlate, (indexOfSearchedPlateI + 1), (parkingSpaceNumber + 1));
                                        }

                                        if (vehicleArray[parkingSpaceNumber, 0] == null && vehicleArray[parkingSpaceNumber, 1] == null)    //Kollar om parkeringen är helt tom och lägger i så fall till MC'n på första platsen i rutan
                                        {
                                            Console.WriteLine("The MC has been added to the parkingspace");
                                            vehicleArray[parkingSpaceNumber, 0] = tempVehicle;
                                            vehicleArray[indexOfSearchedPlateI, indexOfSearchedPlateJ] = null;
                                            Staffreceiptmovevehicle(tempVehicle.LicensePlate, (indexOfSearchedPlateI + 1), (parkingSpaceNumber + 1));
                                        }




                                    }

                                    if (tempVehicle.VehicleTypes == Car)
                                    {
                                        if (vehicleArray[parkingSpaceNumber, 0] == null && vehicleArray[parkingSpaceNumber, 1] == null)
                                        {
                                            Console.WriteLine("The car has been added to the parkingspace");
                                            vehicleArray[parkingSpaceNumber, 0] = tempVehicle;
                                            vehicleArray[indexOfSearchedPlateI, indexOfSearchedPlateJ] = null;
                                            Staffreceiptmovevehicle(tempVehicle.LicensePlate, (indexOfSearchedPlateI + 1), (parkingSpaceNumber+1));
                                        }

                                        else
                                        {
                                            Console.WriteLine("The specified parking is already occupied by either a car or a MC. No vehicle has been moved.");
                                        }
                                    }
                                }

                                if (numInputMoveVehicle == 2)
                                {
                                    Console.WriteLine("You have canceled the choice, no vehicle has moved.");
                                    Console.WriteLine("If you are searching for another vehicle, check spelling, capital/lower case letters or try the search function (Main menu option 4).");
                                    Console.WriteLine("");
                                }
                            }
                            else
                            {
                                Console.WriteLine("A vehicle with a matching licenseplate has not been found.");
                                Console.WriteLine("Did you spell it correctly, capital / lower case letters? ");
                                Console.WriteLine("Try using the search function (Main menu option 4).");
                                Console.WriteLine("");
                            }
                            for (int i = 0; i < 100; i++)       //Skriver ut arrayn till en textfil som sparas "Parking array save file.txt"
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    Vehicle saveTempVehicle = vehicleArray[i, j];
                                    Writetotempfile(saveTempVehicle);
                                }
                            }

                            Writetooriginalfile();

                            Returntomenu();
                            break;
                        }
                    case 3:             //Checkar ut och tar bort ett specifikt fordon ifrån parkeringen
                        {

                            Console.WriteLine("What licensenumber of the vehicle you want to check out?");
                            Console.Write("Please enter licensenumber: ");
                            string licenseNumberSearched = Getlicensenumber();
                            bool parkingFound = false;
                            int price = 0;

                            for (int i = 0; i < 100; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (vehicleArray[i, j] != null)     //Om parkeringen inte är null så kollar vi om den innehåller angivet sökord för regnummer
                                    {
                                        string tempLicensePlate = vehicleArray[i, j].LicensePlate;

                                        if (tempLicensePlate.Contains(licenseNumberSearched) == true)       //Om parkeringsplatsen innehåller en del av det eftersökta regnummret får personal val om det är rätt fordon programmet plockat fram
                                        {
                                            Console.WriteLine("Is {0} the licensenumber of the vehicle you want to check out?", tempLicensePlate);
                                            Console.WriteLine("1. Yes");
                                            Console.WriteLine("2. No");

                                            int numInputSearchVehicle = Inputfailsafetwo();

                                            if (numInputSearchVehicle == 1)     //Om det är rätt regnummer så skriver den ut nödvändig info för att checka ut samt skriver ut två filer som skickas till skrivaren och ges till personal som hämtar bil och kund som skall betala
                                            {
                                                int numberOfMinutes = Calculatetimeparked(vehicleArray[i, j].ParkingTime);

                                                price = Pricecalculate(vehicleArray[i, j].VehicleTypes, numberOfMinutes);

                                                Console.WriteLine("Parking space {0} contains the vehicle you want to check out and it was parked there {1}.", (i + 1), vehicleArray[i, j].ParkingTime); //Snabb presentation av vart den står och när den blev parkerad där
                                                Console.WriteLine("");
                                                Console.Write("Press enter to continue to customer receipt");
                                                Console.ReadKey();
                                                Console.Clear();

                                                Customerreceipt(vehicleArray[i, j].LicensePlate, numberOfMinutes, price);       //Skriver ut kundens kvitto på konsolen

                                                Console.WriteLine("");
                                                Console.Write("Press any key to continue to staff receipt...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                Staffreceiptcheckout(vehicleArray[i, j].LicensePlate, (i + 1));         //Skriver ut en flyttorder på konsolen till personal om att hämta fordonet och lämna det till kunden

                                                vehicleArray[i, j] = null; //Nollställer parkeringsplatsen

                                                parkingFound = true;
                                                i = 99;    //Ser till att programmet inte söker efter nästa fordon som matchar sökningen om ett fordon har valt och redan plockats bort            
                                                Console.WriteLine("");
                                            }
                                            if (numInputSearchVehicle == 2)
                                            {
                                                parkingFound = true;
                                            }
                                        }
                                    }
                                }
                            }

                            if (!parkingFound) //Om det eftersökta regnummret inte finns på parkeringen
                            {
                                Console.WriteLine("The licensenumber has not been found in the parking lot.");
                                Console.WriteLine("Did you spell it correctly, capital/lower case letters?");
                            }

                            for (int i = 0; i < 100; i++)           //Skriver ut arrayn till en textfil som sparas "Parking array save file.txt"
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    Vehicle saveTempVehicle = vehicleArray[i, j];
                                    Writetotempfile(saveTempVehicle);
                                }
                            }

                            Writetooriginalfile();

                            Returntomenu();
                            break;
                        }
                    case 4:             //Sökning efter fordon baserat på regnr
                        {

                            Console.WriteLine("What licensenumber do you want to search for?");
                            Console.Write("Please enter licensenumber: ");
                            string licenseNumberSearched = Getlicensenumber();
                            bool parkingFound = false;
                            int price = 0;

                            for (int i = 0; i < 100; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (vehicleArray[i, j] != null)
                                    {
                                        string tempLicensePlate = vehicleArray[i, j].LicensePlate;

                                        if (tempLicensePlate.Contains(licenseNumberSearched) == true)
                                        {
                                            Console.WriteLine("Is {0} the licensenumber of the vehicle you searched for?", tempLicensePlate);
                                            Console.WriteLine("1. Yes");
                                            Console.WriteLine("2. No");

                                            int numInputSearchVehicle = Inputfailsafetwo();

                                            if (numInputSearchVehicle == 1)
                                            {
                                                int numberOfMinutes = Calculatetimeparked(vehicleArray[i, j].ParkingTime);

                                                price = Pricecalculate(vehicleArray[i, j].VehicleTypes, numberOfMinutes);

                                                Console.WriteLine("Parking space {0} contains the vehicle that you searched for", (i + 1));
                                                Console.WriteLine("The vehicle {0} was parked at the parking lot {1} and has accumulated a fee of {2} koruna so far.", vehicleArray[i, j].LicensePlate, vehicleArray[i, j].ParkingTime, price);
                                                parkingFound = true;
                                            }
                                            if (numInputSearchVehicle == 2)
                                            {
                                                parkingFound = true;
                                            }
                                        }
                                    }
                                }
                            }

                            if (!parkingFound)
                            {
                                Console.WriteLine("The licensenumber has not been found in the parking lot.");
                                Console.WriteLine("Did you spell it correctly, capital/lower case letters? ");
                            }

                            Returntomenu();
                            break;
                        }
                    case 5:             //Hämtar en visuell presentation av parkeringsplatsen
                        {
                            int lotOcupation = 0;
                            int lineRowCount = 1;

                            Console.WriteLine("*************************************************"); //50 st stjärnor

                            for (int i = 0; i < 100; i++)           //For loop som går igenom hela array parkeringen. Om det är en bil eller två MC's skickar den "2", är det en ensam mc skickas "1", är platsen helt tom skickas 0. 
                                                                    //lineRowCount håller reda på vart i array programmet befinner sig. Annars skulle den inte kunna formatera den visuella presetantationen.
                            {
                                if (vehicleArray[i, 0] != null)
                                {
                                    if (vehicleArray[i, 1] != null)
                                    {
                                        lotOcupation = 2;
                                        Printvisualdisplayofparking(lotOcupation, lineRowCount);
                                    }
                                    if (vehicleArray[i, 1] == null)
                                    {
                                        if (vehicleArray[i, 0].VehicleTypes == Car)
                                        {
                                            lotOcupation = 2;
                                            Printvisualdisplayofparking(lotOcupation, lineRowCount);
                                        }
                                        if (vehicleArray[i, 0].VehicleTypes == MC)
                                        {
                                            lotOcupation = 1;
                                            Printvisualdisplayofparking(lotOcupation, lineRowCount);
                                        }
                                    }
                                }

                                if (vehicleArray[i, 0] == null)
                                {
                                    if (vehicleArray[i, 1] != null)
                                    {
                                        lotOcupation = 1;
                                        Printvisualdisplayofparking(lotOcupation, lineRowCount);
                                    }
                                    if (vehicleArray[i, 1] == null)
                                    {
                                        lotOcupation = 0;
                                        Printvisualdisplayofparking(lotOcupation, lineRowCount);
                                    }
                                }
                                lineRowCount++;
                            }
                            Console.WriteLine("*************************************************");

                            Printinfovisualdisplayofparking();
                            Console.WriteLine("");
                            Console.WriteLine("*************************************************"); //50 st stjärnor

                            Returntomenu();
                            break;
                        }
                    case 6:             //Skriver ut en lista på hur beläggningen ser ut på parkeringen.
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                if (vehicleArray[i, 0] == null && vehicleArray[i, 1] == null)
                                {
                                    Console.WriteLine("Parking space {0}: empty", i + 1);
                                }

                                else
                                {
                                    if (vehicleArray[i, 0] != null)
                                    {
                                        Console.Write("Parking space {0}: ", i + 1);
                                        Console.Write(vehicleArray[i, 0].VehicleTypes + " -|- ");
                                        Console.Write("License plate: " + vehicleArray[i, 0].LicensePlate + " -|- ");
                                        Console.WriteLine("Vehicle Parked: "+ vehicleArray[i, 0].ParkingTime);
                                    }
                                    if (vehicleArray[i, 0] == null)
                                    {
                                        Console.WriteLine("First half of parking space {0}: empty", i + 1);
                                    }

                                    if (vehicleArray[i, 1] != null)
                                    {
                                        Console.Write("The second half: ");
                                        Console.Write(vehicleArray[i, 1].VehicleTypes + " -|- ");
                                        Console.Write("License plate: " + vehicleArray[i, 1].LicensePlate + " -|- ");
                                        Console.WriteLine("Vehicle Parked: " + vehicleArray[i, 1].ParkingTime);


                                    }
                                }




                            }

                            Returntomenu();
                            break;
                        }
                    case 7:             //Optimerar parkeringsplatsen enligt val 1. Enbart motorcyklar eller 2. Alla fordon.
                        {
                            Console.WriteLine("Which vehicles do you want to optimize?");                                            //Undermeny för val av optimerings metod
                            Console.WriteLine("1. MC's");
                            Console.WriteLine("2. All vehicles");

                            int optimizeMenu = Inputfailsafetwo();

                            switch (optimizeMenu)
                            {
                                case 1:         //Platsoptimerar en motorcykel
                                    {
                                        Newworkordergeneration();       //Genererar en ny rad i arbetsorder dokumentet så att det blir tydligt att den är en ny sortering och dess flyttjobba som börjar här

                                        int i = 99;
                                        int j = 0;

                                        int indexI = 0;
                                        Vehicle temp = null;

                                        bool wayOut = false;
                                        bool moved = false;

                                        for (i = 99; i >= 0; i--)
                                        {
                                            for (j = 0; j < 2; j++)
                                            {
                                                if (vehicleArray[i, j] != null)
                                                {
                                                    if (vehicleArray[i, j].VehicleTypes == MC)
                                                    {
                                                        temp = vehicleArray[i, j];

                                                        for (indexI = 0; indexI < 100; indexI++)            //Loopar igenom arrayn från början för att se om det finns lediga platser för en MC
                                                        {
                                                            if (i - indexI <= 0)
                                                            {
                                                                Console.WriteLine("All of the motorcycles have been sorted.");
                                                                Console.WriteLine("Press any key to continue to staff work order");
                                                                Console.ReadKey();
                                                                Console.Clear();

                                                                Readfromworkorder();
                                                                Console.WriteLine("");
                                                                wayOut = true;
                                                            }
                                                            if (i - indexI > 0)
                                                            {
                                                                if (vehicleArray[indexI, 0] != null)
                                                                {
                                                                    if (vehicleArray[indexI, 0].VehicleTypes == MC)
                                                                    {
                                                                        if (vehicleArray[indexI, 1] == null)
                                                                        {
                                                                            vehicleArray[indexI, 1] = temp;
                                                                            Printtoworkorder(temp, (i + 1), (indexI+1));
                                                                            vehicleArray[i, j] = null;
                                                                            moved = true;
                                                                        }
                                                                    }
                                                                }
                                                                if (vehicleArray[indexI, 0] == null)
                                                                {
                                                                    vehicleArray[indexI, 0] = temp;
                                                                    Printtoworkorder(temp, (i + 1), (indexI+1));
                                                                    vehicleArray[i, j] = null;
                                                                    moved = true;
                                                                }
                                                            }
                                                            if (wayOut)
                                                            {
                                                                indexI = 100;
                                                                i = -1;
                                                                j = 2;
                                                            }
                                                            if (moved)
                                                            { 
                                                                indexI = 100;
                                                                moved = false;
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                        }

                                        for (i = 0; i < 100; i++)       //Skriver ut arrayn till en textfil som sparas "Parking array save file.txt"
                                        {
                                            for (j = 0; j < 2; j++)
                                            {
                                                Vehicle saveTempVehicle = vehicleArray[i, j];
                                                Writetotempfile(saveTempVehicle);
                                            }
                                        }

                                        Writetooriginalfile();

                                        break;
                                    }
                                case 2:         //Platsoptimerar en bil. Sorterar först för en bil och sedan motorcyklar. Detta för att generera så lite jobb som möjligt för personalen
                                    {
                                        Newworkordergeneration();       //Genererar en ny rad i arbetsorder dokumentet så att det blir tydligt att den är en ny sortering och dess flytt jobb som börjar här

                                        bool wayOutCar = false; //Håller koll på om alla bilar har sorterats
                                        bool movedCar = false;  //Håller koll på om en bil har lagts så långt fram det går
                                        Vehicle temp = null;

                                        int secondIndex = 0;    

                                        for (int index = 99; index >= 0; index--) //Loopar igenom array bakifrån för att generera så få flyttar som möjligt
                                        {
                                            if (vehicleArray[index, 0] != null)
                                            {
                                                if (vehicleArray[index, 0].VehicleTypes == Car)
                                                {
                                                    temp = vehicleArray[index, 0];

                                                    for (secondIndex = 0; secondIndex < 100; secondIndex++)            //Loopar igenom arrayn från början för att se om det finns lediga platser för en bil
                                                    {
                                                        if (index - secondIndex <= 0)
                                                        {
                                                            wayOutCar = true;
                                                        }
                                                        if (index - secondIndex > 0)
                                                        {
                                                            if (vehicleArray[secondIndex, 0] == null && vehicleArray[secondIndex, 1] == null)
                                                            {
                                                                vehicleArray[secondIndex, 0] = temp;                
                                                                Printtoworkorder(temp, (index + 1), (secondIndex + 1));
                                                                vehicleArray[index, 0] = null;
                                                                movedCar = true;
                                                            }

                                                        }
                                                        if (wayOutCar)
                                                        {
                                                            secondIndex = 100;
                                                            index = -1;
                                                        }
                                                        if (movedCar)
                                                        {
                                                            secondIndex = 100;
                                                            movedCar = false;
                                                        }
                                                    }

                                                }
                                            }
                                        }

                                        int i = 99;
                                        int j = 0;

                                        int indexI = 0;
                                        temp = null;

                                        bool wayOut = false;        //Håller koll på om alla mc's har sorterats
                                        bool moved = false;         //Håller koll på om en MC har flyttats så långt fram de går

                                        for (i = 99; i >= 0; i--)
                                        {
                                            for (j = 0; j < 2; j++)
                                            {
                                                if (vehicleArray[i, j] != null)
                                                {
                                                    if (vehicleArray[i, j].VehicleTypes == MC)
                                                    {
                                                        temp = vehicleArray[i, j];

                                                        for (indexI = 0; indexI < 100; indexI++)            //Loopar igenom arrayn från början för att se om det finns lediga platser för en MC
                                                        {
                                                            if (i - indexI <= 0)
                                                            {
                                                                wayOut = true;
                                                            }
                                                            if (i - indexI > 0)
                                                            {
                                                                if (vehicleArray[indexI, 0] != null)
                                                                {
                                                                    if (vehicleArray[indexI, 0].VehicleTypes == MC)
                                                                    {
                                                                        if (vehicleArray[indexI, 1] == null)
                                                                        {
                                                                            vehicleArray[indexI, 1] = temp;
                                                                            Printtoworkorder(temp, (i + 1), (indexI + 1));
                                                                            vehicleArray[i, j] = null;
                                                                            moved = true;
                                                                        }
                                                                    }
                                                                }
                                                                if (vehicleArray[indexI, 0] == null)
                                                                {
                                                                    vehicleArray[indexI, 0] = temp;
                                                                    Printtoworkorder(temp, (i + 1), (indexI + 1));
                                                                    vehicleArray[i, j] = null;
                                                                    moved = true;
                                                                }
                                                            }
                                                            if (wayOut)
                                                            {
                                                                indexI = 100;
                                                                i = -1;
                                                                j = 2;
                                                            }
                                                            if (moved)
                                                            {
                                                                indexI = 100;
                                                                moved = false;
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                        }

                                        Console.WriteLine("All of the vehicles have been sorted.");
                                        Console.WriteLine("Press any key to continue to staff work order");
                                        Console.ReadKey();
                                        Console.Clear();

                                        Readfromworkorder();
                                        Console.WriteLine("");

                                        for (i = 0; i < 100; i++)       //Skriver ut arrayn till en textfil som sparas "Parking array save file.txt"
                                        {
                                            for (j = 0; j < 2; j++)
                                            {
                                                Vehicle saveTempVehicle = vehicleArray[i, j];
                                                Writetotempfile(saveTempVehicle);
                                            }
                                        }

                                        Writetooriginalfile();

                                        break;
                                    }
                            }

                            Returntomenu();
                            break;
                        }
                    case 8:             //Avslutar programmet
                        {
                            Console.Write("You are now exiting the program.");
                            Console.WriteLine("Are you sure you want to exit?");
                            Console.WriteLine("1. Yes, exit.");
                            Console.WriteLine("2. No, back to main menu.");

                            int noExitBack = 0;

                            noExitBack = Inputfailsafetwo();

                            if (noExitBack == 1)            //Hanterar om personal vill avsluta programmet
                            {
                                Console.Clear();
                                Presentasciiartlasttime();
                                Console.WriteLine("Thank you for using Prague Parking 2.0 © Which is a licensed product from Anton Asplund INC. All rights reserved.");
                                Console.ReadKey();
                                switchCaseLoop = false;

                                for (int i = 0; i < 100; i++)       //Skriver ut arrayn till en textfil som sparas "Parking array save file.txt"
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        Vehicle saveTempVehicle = vehicleArray[i, j];
                                        Writetotempfile(saveTempVehicle);
                                    }
                                }

                                Writetooriginalfile();
                            }

                            if (noExitBack == 2)            //Hanterar om personal inte vill avsluta programmet
                            {
                                Returntomenu();
                            }

                            break;
                        }
                }


            } while (switchCaseLoop);
        }

        static int Inputfailsafetwo()       //Tar hand om inmatning av 2 vals meny, yes/no menyer
        {
            bool failsafe = false;
            int inputNumber = 0;

            Console.Write("Enter menu choice: ");

            while (!failsafe)
            {
                failsafe = int.TryParse(Console.ReadLine(), out inputNumber);
                if (!failsafe)
                {
                    Console.Write("Please enter a valid number: ");
                }
                if (inputNumber < 1 || inputNumber > 2)
                {
                    Console.Write("Please enter a valid number: ");
                    failsafe = false;
                }
            }
            return inputNumber;
        }       

        static void Returntomenu()      //Skriver ut en fras och snyggar till konoslfönsret innan en tillbakagång till huvudmenyn görs
        {
            Console.Write("Please press any key to continue to main menu...");
            Console.ReadKey();
            Console.Clear();
        }      

        static int Presentswitchcasemenu()      //Skriver ut huvudmenyn samt programlogga
        {

            Presentasciiart();

            Console.WriteLine("Menu: ");                                            //Meny för programmet och dess funktioner
            Console.WriteLine("1. Add a new vehicle to parking ");
            Console.WriteLine("2. Move a parked vehicle to a new parking spot");
            Console.WriteLine("3. Check out and remove a parked vehicle");
            Console.WriteLine("4. Search for a vehicle through license plate");
            Console.WriteLine("5. Print a visual display of the parking lot");
            Console.WriteLine("6. Print a sheet to the console of the current status of the parking lot");
            Console.WriteLine("7. Optimization of the parking lot");
            Console.WriteLine("8. Exit the program");
            Console.WriteLine("");

            Console.Write("Please choose an option: ");

            int switchCaseMenu = 0;
            bool failsafe = false;

            while (!failsafe)                   //Felhantering vid inmatning och ser till så att meny val görs så att användaren håller sig inom ramarna för programmet
            {
                failsafe = int.TryParse(Console.ReadLine(), out switchCaseMenu);
                if (!failsafe)
                {
                    Console.WriteLine("Nonexsisting menu choice or invalid number");
                    Console.Write("Try again: ");
                }
                if (switchCaseMenu < 0 || switchCaseMenu > 8)
                {
                    Console.Write("Please enter a value that has a corresponding menu choice. Try again: ");
                    failsafe = false;
                }
            }

            Console.Clear();
            return switchCaseMenu;
        }  

        static string Getlicensenumber()    //Hanterar inmatning av ett registreringsnummer som max får vara 10 bokstäver i valfritt unicode språk NSinSum typsnitt bör användas på konsol fönstret
                                             //För att en del text ska kunna visas, men systemet hanterar det ändå
        {

            string searchedLicenseNumber = "";
            bool failsafe = true;

            while (failsafe)
            {
                failsafe = false;
                searchedLicenseNumber = Console.ReadLine();
                if (searchedLicenseNumber.Length > 10)
                {
                    failsafe = true;
                    Console.Write("The licensenumber is too long(Max 10 letters). Please try again: ");
                }
                if (searchedLicenseNumber.Length <= 0)
                {
                    failsafe = true;
                    Console.Write("The licensenumber is too short(Atleast 1 letter). Please try again");
                }

            }

            Console.Clear();
            return searchedLicenseNumber;
        }   

        static VehicleTypeEnum Getvehicletype()     // KOllar om det är en bil eller MC vid tilläggning av nytt fordon
        {
            Console.WriteLine("Is the vehicle a car or a mc?");
            Console.WriteLine("1: Car");
            Console.WriteLine("2: MC");

            int menuchoice = Inputfailsafetwo();
            VehicleTypeEnum getVehicleType = VehicleTypeEnum.Car;

            if (menuchoice == 1)
            {
                getVehicleType = VehicleTypeEnum.Car;
            }

            if (menuchoice == 2)
            {
                getVehicleType = VehicleTypeEnum.MC;
            }

            Console.Clear();
            return getVehicleType;
        }

        static int Inputfailsafeonehundred()        //Hanterar inmatning av val av parkeringsplats
        {
            bool failsafe = false;
            int inputNumber = 0;

            while (!failsafe)
            {
                failsafe = int.TryParse(Console.ReadLine(), out inputNumber);
                if (!failsafe)
                {
                    Console.Write("Please enter a valid number: ");
                }
                if (inputNumber < 1 || inputNumber > 100)
                {
                    Console.Write("Please enter a valid number: ");
                    failsafe = false;
                }
            }
            return inputNumber;
        }   

        static void Writetotempfile(Vehicle tempParking) //Skriver ut fordon till en temp fil
        {
            StreamWriter writeFileToParkingArrayTemp = new StreamWriter("Temp parking array save file.txt", true);

            using (writeFileToParkingArrayTemp)
            {
                if (tempParking != null)
                {
                    writeFileToParkingArrayTemp.WriteLine(tempParking.VehicleTypes);
                    writeFileToParkingArrayTemp.WriteLine(tempParking.LicensePlate);
                    writeFileToParkingArrayTemp.WriteLine(tempParking.ParkingTime);
                }
                if (tempParking == null)
                {
                    writeFileToParkingArrayTemp.WriteLine();
                    writeFileToParkingArrayTemp.WriteLine();
                    writeFileToParkingArrayTemp.WriteLine();
                }
            }
        }

        static void Writetooriginalfile()       //Skriver från tempfil till orginalfil och tar bort temp filen
        {


            StreamWriter writeFileToParkingArray = new StreamWriter("Parking array save file.txt", false);
            StreamReader readFileOfParkingArrayTemp = new StreamReader("Temp parking array save file.txt");

            using (writeFileToParkingArray)
            {
                using (readFileOfParkingArrayTemp)
                {
                    string temp = readFileOfParkingArrayTemp.ReadLine();
                    while (temp != null)
                    {
                        writeFileToParkingArray.WriteLine(temp);
                        temp = readFileOfParkingArrayTemp.ReadLine();
                    }
                }
            }

            File.Delete("Temp parking array save file.txt");
        }   

        static void Presentasciiart()       //Skriver ut programlogga i huvudmenyn
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("__________                                      ");
            Console.WriteLine("\\______   \\____________     ____  __ __   ____  ");
            Console.WriteLine(" |     ___/\\_  __ \\__  \\   / ___\\|  |  \\_/ __ \\ ");
            Console.WriteLine(" |    |     |  | \\// __ \\_/ /_/  >  |  /\\  ___/ ");
            Console.WriteLine(" |____|     |__|  (____  /\\___  /|____/  \\___  >");
            Console.WriteLine("                       \\//_____/             \\/ ");
            Console.WriteLine("__________               __   .__                ");
            Console.WriteLine("\\______   \\_____ _______|  | _|__| ____    ____  ");
            Console.WriteLine(" |     ___/\\__  \\_  __  \\  |/ /  |/    \\  / ___\\ ");
            Console.WriteLine(" |    |     / __ \\|  | \\/    <|  |   |  \\/ /_/  >");
            Console.WriteLine(" |____|    (____  /__|  |__|_ \\__|___|  /\\___  / ");
            Console.WriteLine("                \\/           \\/       \\//_____/  ");
            Console.WriteLine("____   ____         ________      _______   ");
            Console.WriteLine("\\   \\ /   /         \\_____  \\     \\   _  \\  ");
            Console.WriteLine(" \\   Y   /   ______  /  ____/     /  /_\\  \\ ");
            Console.WriteLine("  \\     /   /_____/ /       \\     \\  \\_/   \\ ");
            Console.WriteLine("   \\___/            \\_______ \\ /\\ /\\_____  /");
            Console.WriteLine("                            \\/ \\/ \\/     \\/ ");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Gray;
        }      

        static void Presentasciiartfirsttime()      //Skriver ut programlogga vid startup av programmet
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            int milliseconds = 200;     
            Thread.Sleep(1000);         
            Console.WriteLine("__________                                      ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("\\______   \\____________     ____  __ __   ____  ");
            Thread.Sleep(milliseconds);
            Console.WriteLine(" |     ___/\\_  __ \\__  \\   / ___\\|  |  \\_/ __ \\ ");
            Thread.Sleep(milliseconds);
            Console.WriteLine(" |    |     |  | \\// __ \\_/ /_/  >  |  /\\  ___/ ");
            Thread.Sleep(milliseconds);
            Console.WriteLine(" |____|     |__|  (____  /\\___  /|____/  \\___  >");
            Thread.Sleep(milliseconds);
            Console.WriteLine("                       \\//_____/             \\/ ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("__________               __   .__                ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("\\______   \\_____ _______|  | _|__| ____    ____  ");
            Thread.Sleep(milliseconds);
            Console.WriteLine(" |     ___/\\__  \\_  __  \\  |/ /  |/    \\  / ___\\ ");
            Thread.Sleep(milliseconds);
            Console.WriteLine(" |    |     / __ \\|  | \\/    <|  |   |  \\/ /_/  >");
            Thread.Sleep(milliseconds);
            Console.WriteLine(" |____|    (____  /__|  |__|_ \\__|___|  /\\___  / ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("                \\/           \\/       \\//_____/  ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("____   ____         ________      _______   ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("\\   \\ /   /         \\_____  \\     \\   _  \\  ");
            Thread.Sleep(milliseconds);
            Console.WriteLine(" \\   Y   /   ______  /  ____/     /  /_\\  \\ ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("  \\     /   /_____/ /       \\     \\  \\_/   \\ ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("   \\___/            \\_______ \\ /\\ /\\_____  /");
            Thread.Sleep(milliseconds);
            Console.WriteLine("                            \\/ \\/ \\/     \\/ ");
            Thread.Sleep(milliseconds);
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Gray;
        }   

        static void Presentasciiartlasttime()       //Skriver ut programlogga vid avslutning av programmet samt tar bort filen för dagens arbetsordrar.
        {
            File.Delete("Workorder Optimization.txt");      //Tar bort den gammla filen för arbetsorder

            int count = 1;

            for (int i = 0; i < 8; i++)
            {
                if (count % 2 == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;

                    Console.WriteLine("__________                                      ");
                    Console.WriteLine("\\______   \\____________     ____  __ __   ____  ");
                    Console.WriteLine(" |     ___/\\_  __ \\__  \\   / ___\\|  |  \\_/ __ \\ ");
                    Console.WriteLine(" |    |     |  | \\// __ \\_/ /_/  >  |  /\\  ___/ ");
                    Console.WriteLine(" |____|     |__|  (____  /\\___  /|____/  \\___  >");
                    Console.WriteLine("                       \\//_____/             \\/ ");
                    Console.WriteLine("__________               __   .__                ");
                    Console.WriteLine("\\______   \\_____ _______|  | _|__| ____    ____  ");
                    Console.WriteLine(" |     ___/\\__  \\_  __  \\  |/ /  |/    \\  / ___\\ ");
                    Console.WriteLine(" |    |     / __ \\|  | \\/    <|  |   |  \\/ /_/  >");
                    Console.WriteLine(" |____|    (____  /__|  |__|_ \\__|___|  /\\___  / ");
                    Console.WriteLine("                \\/           \\/       \\//_____/  ");
                    Console.WriteLine("____   ____         ________      _______   ");
                    Console.WriteLine("\\   \\ /   /         \\_____  \\     \\   _  \\  ");
                    Console.WriteLine(" \\   Y   /   ______  /  ____/     /  /_\\  \\ ");
                    Console.WriteLine("  \\     /   /_____/ /       \\     \\  \\_/   \\ ");
                    Console.WriteLine("   \\___/            \\_______ \\ /\\ /\\_____  /");
                    Console.WriteLine("                            \\/ \\/ \\/     \\/ ");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Thank you for using Prague Parking 2.0 © Which is a licensed product from Anton Asplund INC. All rights reserved.");

                    Thread.Sleep(500);          //Fixar längden på blinkningarna
                    Console.Clear();
                    count++;
                }
                if (count % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                    Console.WriteLine("__________                                      ");
                    Console.WriteLine("\\______   \\____________     ____  __ __   ____  ");
                    Console.WriteLine(" |     ___/\\_  __ \\__  \\   / ___\\|  |  \\_/ __ \\ ");
                    Console.WriteLine(" |    |     |  | \\// __ \\_/ /_/  >  |  /\\  ___/ ");
                    Console.WriteLine(" |____|     |__|  (____  /\\___  /|____/  \\___  >");
                    Console.WriteLine("                       \\//_____/             \\/ ");
                    Console.WriteLine("__________               __   .__                ");
                    Console.WriteLine("\\______   \\_____ _______|  | _|__| ____    ____  ");
                    Console.WriteLine(" |     ___/\\__  \\_  __  \\  |/ /  |/    \\  / ___\\ ");
                    Console.WriteLine(" |    |     / __ \\|  | \\/    <|  |   |  \\/ /_/  >");
                    Console.WriteLine(" |____|    (____  /__|  |__|_ \\__|___|  /\\___  / ");
                    Console.WriteLine("                \\/           \\/       \\//_____/  ");
                    Console.WriteLine("____   ____         ________      _______   ");
                    Console.WriteLine("\\   \\ /   /         \\_____  \\     \\   _  \\  ");
                    Console.WriteLine(" \\   Y   /   ______  /  ____/     /  /_\\  \\ ");
                    Console.WriteLine("  \\     /   /_____/ /       \\     \\  \\_/   \\ ");
                    Console.WriteLine("   \\___/            \\_______ \\ /\\ /\\_____  /");
                    Console.WriteLine("                            \\/ \\/ \\/     \\/ ");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Thank you for using Prague Parking 2.0 © Which is a licensed product from Anton Asplund INC. All rights reserved.");

                    Thread.Sleep(500);      //Fixar längden på blinkningarna
                    Console.Clear();
                    count++;
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("__________                                      ");
            Console.WriteLine("\\______   \\____________     ____  __ __   ____  ");
            Console.WriteLine(" |     ___/\\_  __ \\__  \\   / ___\\|  |  \\_/ __ \\ ");
            Console.WriteLine(" |    |     |  | \\// __ \\_/ /_/  >  |  /\\  ___/ ");
            Console.WriteLine(" |____|     |__|  (____  /\\___  /|____/  \\___  >");
            Console.WriteLine("                       \\//_____/             \\/ ");
            Console.WriteLine("__________               __   .__                ");
            Console.WriteLine("\\______   \\_____ _______|  | _|__| ____    ____  ");
            Console.WriteLine(" |     ___/\\__  \\_  __  \\  |/ /  |/    \\  / ___\\ ");
            Console.WriteLine(" |    |     / __ \\|  | \\/    <|  |   |  \\/ /_/  >");
            Console.WriteLine(" |____|    (____  /__|  |__|_ \\__|___|  /\\___  / ");
            Console.WriteLine("                \\/           \\/       \\//_____/  ");
            Console.WriteLine("____   ____         ________      _______   ");
            Console.WriteLine("\\   \\ /   /         \\_____  \\     \\   _  \\  ");
            Console.WriteLine(" \\   Y   /   ______  /  ____/     /  /_\\  \\ ");
            Console.WriteLine("  \\     /   /_____/ /       \\     \\  \\_/   \\ ");
            Console.WriteLine("   \\___/            \\_______ \\ /\\ /\\_____  /");
            Console.WriteLine("                            \\/ \\/ \\/     \\/ ");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Gray;

        }       

        static Vehicle Getinfofromsavefile(int lineToSkip)      //Hämtar parkeringsdata ifrån en sparfil och matar in den i programmet. Ser till att programmet startar upp med samma värden där det senast nyttjades
        {
            Vehicle tempVehicle = null;

            try
            {
                StreamReader saveFile = new StreamReader("Parking array save file.txt");

                try
                {
                    string vehicleTypeString = "";
                    string licensPlate = "";
                    string timeStamp = "";

                    VehicleTypeEnum vehicleType = VehicleTypeEnum.MC;

                    using (saveFile)
                    {
                        int i = 0;

                        while (i <= lineToSkip)
                        {
                            vehicleTypeString = saveFile.ReadLine();
                            licensPlate = saveFile.ReadLine();
                            timeStamp = saveFile.ReadLine();
                            i = i + 3;
                        }
                    }
                    if (vehicleTypeString != "")
                    {
                        if (vehicleTypeString == "MC")
                        {
                            vehicleType = VehicleTypeEnum.MC;
                        }
                        if (vehicleTypeString == "Car")
                        {
                            vehicleType = VehicleTypeEnum.Car;
                        }

                        DateTime parkingTime = Convert.ToDateTime(timeStamp);

                        tempVehicle = new Vehicle(vehicleType, licensPlate, parkingTime);
                    }

                }
                finally
                {
                    saveFile.Close();
                }
            }
            catch (FileNotFoundException)
            {
                if (lineToSkip == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("*WARNING* The save file could not be found, no vehicles from previous backup has been added *WARNING");
                        Thread.Sleep(50);
                        Console.Clear();
                        Thread.Sleep(50);
                    }
                    Console.WriteLine("*WARNING* The save file could not be found, no vehicles from previous backup has been added *WARNING");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            catch (DirectoryNotFoundException)
            {
                if (lineToSkip == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("*WARNING* The directory of the save file could not be found, no vehicles from previous backup has been added *WARNING");
                        Thread.Sleep(50);
                        Console.Clear();
                        Thread.Sleep(50);
                    }
                    Console.WriteLine("*WARNING* The directory of the save file could not be found, no vehicles from previous backup has been added *WARNING");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }
            catch
            {
                if (lineToSkip == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("*WARNING* No save data has been loaded. *WARNING");
                        Thread.Sleep(50);
                        Console.Clear();
                        Thread.Sleep(50);
                    }
                    Console.WriteLine("*WARNING* No save data has been loaded. *WARNING");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

            return tempVehicle;
        }   

        static int Calculatetimeparked(DateTime parkedTimeStamp)        //Räknar hur hur länge fordonet har varit parkerat och skickar tillbaka antalet minuter
        {
            int numberOfMinutes;
            DateTime checkOut = DateTime.Now;

            TimeSpan interval = checkOut - parkedTimeStamp;

            string temp = interval.TotalMinutes.ToString();

            decimal numberOfMinutes2 = decimal.Parse(temp);
            numberOfMinutes = (int)Math.Ceiling(numberOfMinutes2);


            return numberOfMinutes;
        }  

        static int MCpricecalculate(int numberOfMinutes)        //Räknar ut pris för det parkerade fordonet. Tar hänsyn till de 5 första minuterna samt om det är längre än 2 timmar
        {

            decimal numberOfMinutesFractional = Convert.ToDecimal(numberOfMinutes);

            int price = 0;

            if (numberOfMinutes < 5)
            {
                return price;
            }

            numberOfMinutes = numberOfMinutes - 5;      //De första 5 minuterna är alltid gratis enligt uppgift och förtydligande enligt lärare

            if (numberOfMinutes < 120)
            {
                price = 20;
                return price;
            }
            decimal sixty = 60;
            decimal numberOfHours = numberOfMinutesFractional / sixty;


            numberOfHours = Math.Ceiling(numberOfHours);

            price = (int)numberOfHours * 10;

            return price;
        }       

        static int Carpricecalculate(int numberOfMinutes)       //Räknar ut pris för det parkerade fordonet. Tar hänsyn till de 5 första minuterna samt om det är längre än 2 timmar
        {
            decimal numberOfMinutesFractional = Convert.ToDecimal(numberOfMinutes);

            int price = 0;

            if (numberOfMinutes < 5)
            {
                return price;
            }

            numberOfMinutes = numberOfMinutes - 5;      //De första 5 minuterna är alltid gratis enligt uppgift och förtydligande enligt lärare

            if (numberOfMinutes < 120)
            {
                price = 40;
                return price;
            }
            decimal sixty = 60;
            decimal numberOfHours = numberOfMinutesFractional / sixty;


            numberOfHours = Math.Ceiling(numberOfHours);

            price = (int)numberOfHours * 20;

            return price;
        }           

        static int Pricecalculate(VehicleTypeEnum vehicle, int numberOfMinutes) //Räknar ut pris för det parkerade fordonet. Tar hänsyn till de 5 första minuterna samt om det är längre än 2 timmar
        {
            int price = 0;

            if (vehicle == VehicleTypeEnum.MC)
            {
                price = MCpricecalculate(numberOfMinutes);
            }
            if (vehicle == VehicleTypeEnum.Car)
            {
                price = Carpricecalculate(numberOfMinutes);
            }

            return price;
        }       

        static void Customerreceipt(string license, int totalTimeParked, int totalCost)     //Skriver ut kvitto till konsolen för kunden som hämtar ett fordon
        {
            int hours = totalTimeParked / 60;
            int minutes = totalTimeParked % 60;

            Console.WriteLine("{0,40}", "Customers Receipt");
            Console.WriteLine("******************************************************************");
            Console.WriteLine("*License number {0,20}{1,30}", "Time Parked", "Amount Due*");
            Console.WriteLine("******************************************************************");
            Console.WriteLine("{0}{1,20} hours and {2} minutes {3,15} koruna", license, hours, minutes, totalCost);
        }      

        static void Staffreceiptcheckout(string license, int parkingLot)                         //Skriver ut kvitto til konsolen för personal som ska flytta fordon till kund
        {
            Console.WriteLine("{0,40}", "Staff Receipt");
            Console.WriteLine("******************************************************************");
            Console.WriteLine("*Parking lot {0,20}{1,30}  *", "License number", "Where to");
            Console.WriteLine("******************************************************************");
            Console.WriteLine("{0}{1,30} {2,30}", parkingLot, license, "Customer");
        }
        static void Staffreceiptmovevehicle(string license, int parkingLot, int whereTo)        //Skriver ut kvitto till konsolen för personal som ska flytta fordon till annan parkeringsplats
        {
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue to staff receipt");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("{0,40}", "Staff Receipt");
            Console.WriteLine("******************************************************************");
            Console.WriteLine("*Parking lot {0,20}{1,30}  *", "License number", "Where to");
            Console.WriteLine("******************************************************************");
            Console.WriteLine("{0}{1,30} {2,30}", parkingLot, license, whereTo);
            Console.WriteLine("");
        }

        static void Printvisualdisplayofparking(int lotOccupation, int lineRowCount)            //Skriver ut visuell presenation av parkeringen. Blå för en mc. Grön för tom plats och rött för 2 mc's eller en bil.
        {
            if (lineRowCount % 10 == 1)
            {
                Console.Write("*|");
            }

            if (lotOccupation == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.Write("|X|");

                Console.ForegroundColor = ConsoleColor.Gray;
            }
            if (lotOccupation == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;

                Console.Write("|X|");

                Console.ForegroundColor = ConsoleColor.Gray;
            }
            if (lotOccupation == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Write("|X|");

                Console.ForegroundColor = ConsoleColor.Gray;
            }

            if (lineRowCount % 10 == 0)
            {
                Console.Write("|* ( {0} - {1} ) ", (lineRowCount - 9), lineRowCount);
                if (lineRowCount == 10)
                {
                    Console.WriteLine("  *");
                }
                if (10 < lineRowCount && lineRowCount < 99)
                {
                    Console.WriteLine(" *");
                }
                if (lineRowCount == 100)
                {
                    Console.WriteLine("*");
                }
            }
        }

        static void Printinfovisualdisplayofparking()           //Skriver ut förklaring av hur man skall tolka den visuella presenationen 
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Green ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("- Parking lot empty");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Darkcyan ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("- Parking lot occupied by 1 motorcycle");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Red");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("- Parking lot is occupied wither by a car or 2 motorcyles");
        }

        static void Printtoworkorder(Vehicle temp, int fromParking, int toParking)      //Skriver ut en arnetsorder till fil när platsoptimering fordon skall ske. Bra att ha som backup för personal!
        {
            StreamWriter workOrder = new StreamWriter("Workorder Optimization.txt", true);

            using (workOrder)
            {
                workOrder.WriteLine("------------------------------------------------------------");
                workOrder.Write(temp.LicensePlate);
                workOrder.WriteLine(" - Move from parkinglot number {0} and park at {1}", fromParking, toParking);
                

            }
        }

        static void Readfromworkorder()         //Hämtar hela listan för arbetsorder vid flytt av fordon och skriver ut den till konsolen OBS att alla arbetsordrar ifrån dagens optimeringar skriv ut på konslen. Arbetsorder tas bort vid ett korrekt avslut av programmet.
        {
            try
            {
                StreamReader workOrder = new StreamReader("Workorder Optimization.txt");
                try
                {
                    string temp = workOrder.ReadLine();
                    while (temp != null)
                    {
                        Console.WriteLine(temp);
                        temp = workOrder.ReadLine();
                    }
                }
                finally
                {
                    workOrder.Close();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\"Workorder.txt\" has not been found");
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory of \"Workorder.txt\" has not been found");
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("Something went wrong when trying to read from \"Workorder.txt\"");
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }

        static void Newworkordergeneration()        //Skriver till textfilen för arbetsordrar som indikerar och skriver att en ny arbetsorder har påbörjats. Skriver även ut en tidstämpel för personal referens
        {
            StreamWriter workOrder = new StreamWriter("Workorder Optimization.txt", true);

            using (workOrder)
            {
                workOrder.WriteLine("************************************************************");
                workOrder.WriteLine("*A new work order has been created({0})", DateTime.Now);
                workOrder.WriteLine("************************************************************");
            }
        }

        


    }
}
