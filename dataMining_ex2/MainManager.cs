using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public class MainManager
    {
        private MainManagerLogic managerLogic = new MainManagerLogic();
        private List<string> startMenuOptions;
        private List<string> actionMenuOptions;

        private List<string> subMenuOptions;
        private List<string> testFilesNames;

        public delegate int DelegateExecutePresentMenu(string title, List<string> menuOptions);
        public delegate void DelegateExecuteManualAlgorithm();

        public MainManager()
        {
            createMenuesOptions();
            createTestFilesNames();
        }

        private void createTestFilesNames()
        {
            testFilesNames = createOptionsList("testFilesForRegularDBSCAN", "testFilesForKPCA", "testFilesForRandomProjection" );
        }

        public void Run()
        {
            int chosenCommand;

            // start menu calls
            chosenCommand = executePresentMenuOption(presentManu, MenuStrings.StartMenuTitle, startMenuOptions); //executeStartMenuOption();

            // if choose 'Exit'
            if (isChooseExit(chosenCommand, startMenuOptions))
            {
                return;
            }

            executeChosenActionStartMenu(chosenCommand);

            // action manu calls
            Console.Clear();
            chosenCommand = executePresentMenuOption(presentManu, MenuStrings.ActionMenuTitle, actionMenuOptions);

            // if choose 'Exit'
            if (isChooseExit(chosenCommand, actionMenuOptions))
            {
                return;
            }
            printRunning();
            DataSetsGenerator.createDataSet();

            executeChosenActionMenu(chosenCommand);

            Console.WriteLine(MenuStrings.ResultFileLocation);

        }


        #region Action Menu Functions
        private void executeChosenActionMenu(int chosenCommand)
        {
            Console.Clear();

            switch (chosenCommand)
            {
                case 1:
                    executeSubMenuCommand(manualDBSCAN, TestsFilesIndex.REGULAR_DBSCAN);
                    break;
                case 2:
                    executeSubMenuCommand(manualKPCA, TestsFilesIndex.KPCA_DBSCAN);
                    break;
                case 3:
                    executeSubMenuCommand(manualRandomProjection, TestsFilesIndex.RANDOM_PROJECTION_DBSCAN);
                    break;
            }
        }

        private void executeSubMenuCommand(DelegateExecuteManualAlgorithm manualAlgoritmChosen, TestsFilesIndex index)
        {
            Console.Clear();
            subMenuOptions = createOptionsList(MenuStrings.ManualOption, MenuStrings.TestsOptions);
            int command = executePresentMenuOption(presentManu, MenuStrings.SubMenuTitle, subMenuOptions);

            switch (command)
            {
                case 1:
                    manualAlgoritmChosen();
                    break;
                case 2:
                    TestDBSCANChosen(index);
                    break;
            }
        }

        private void executeSubMenuManualCommand(DelegateExecuteManualAlgorithm runReduceAlgoFirst, ReduseDimentionAlgorithmType algorithmType)
        {
            Console.Clear();
            string runDBSCANonLastResultStr = MenuStrings.RunDBSCANonLastResult + "\n" + MenuStrings.Note;
            subMenuOptions = createOptionsList(MenuStrings.RunReduceAlgoFirst, runDBSCANonLastResultStr);
            int command = executePresentMenuOption(presentManu, MenuStrings.SubMenuTitle, subMenuOptions);

            switch (command)
            {
                case 1:
                    runReduceAlgoFirst();
                    break;
                case 2:
                    runDBSCANOnLastAlgoritmResult(algorithmType);
                    break;
            }
        }

        private void manualDBSCAN()
        {
            Tuple<int, double> paramtersForDBSCAN = getDBSCANParamsFromUser();
            printRunning();
            managerLogic.ExecuteDBSCANAndStatistics(paramtersForDBSCAN.Item1, paramtersForDBSCAN.Item2);
        }

        private void manualKPCA()
        {
            executeSubMenuManualCommand(runKPCAbeforeDBSCAN, ReduseDimentionAlgorithmType.KPCA);
        }

        private void manualRandomProjection()
        {
            executeSubMenuManualCommand(runRandomProjectionBeforeDBSCAN, ReduseDimentionAlgorithmType.RANDOM_PROJECTION);
        }

        private void runRandomProjectionBeforeDBSCAN()
        {
            int numberOfDimentions = getNumberOfDimentionsFromUser();
            Tuple<int, double> paramtersForDBSCAN = getDBSCANParamsFromUser();

            RandomProjection randomProjection = new RandomProjection(numberOfDimentions);
            printRunning();
            managerLogic.RunDBSCANWithDimentionReduceAlgorithm(randomProjection.RunRandomProjection, paramtersForDBSCAN.Item1, paramtersForDBSCAN.Item2);
        }

        private void runKPCAbeforeDBSCAN()
        {
            int numberOfDimentions = getNumberOfDimentionsFromUser();
            Tuple<int, double> paramtersForDBSCAN = getDBSCANParamsFromUser();

            KPCA kpca = new KPCA(numberOfDimentions);
            printRunning();
            managerLogic.RunDBSCANWithDimentionReduceAlgorithm(kpca.RunKPCA, paramtersForDBSCAN.Item1, paramtersForDBSCAN.Item2);
        }

        // only if you already have kpcaResult.txt file or random
        private void runDBSCANOnLastAlgoritmResult(ReduseDimentionAlgorithmType algorithmType)
        {
            Tuple<int, double> paramtersForDBSCAN = getDBSCANParamsFromUser();
            string filePath = getFilePathOfLastAlgorithm(algorithmType);
            printRunning();
            managerLogic.UpdateDataSetAndExecuteDBSCANAndStatistics(paramtersForDBSCAN.Item1, paramtersForDBSCAN.Item2, filePath);
        }

        private void TestDBSCANChosen(TestsFilesIndex indexOfFileToRun)
        {
            string[] files = Directory.GetFiles(testFilesNames[(int)indexOfFileToRun], "*.txt", SearchOption.AllDirectories);
            int minPtsIndex = 0, epsIndex = 1, dimNumIndex = 2;
            int minPts, dimentionsNum;
            double eps;
            string fileName = "testResult";

            printRunning();

            if (TestsFilesIndex.REGULAR_DBSCAN == indexOfFileToRun)
            {
                int index = 1;
                foreach (var file in files)
                {
                    DataSetsGenerator.createDataSet();
                    List<string> paramsForAlgo = FileUtils.getParamsFromFile(file);
                    minPts = int.Parse(paramsForAlgo[minPtsIndex]);
                    eps = double.Parse(paramsForAlgo[epsIndex]);
                    managerLogic.ExecuteDBSCANAndStatistics(minPts, eps, fileName + index + ".txt");
                    index++;
                }
            }
            else if (TestsFilesIndex.KPCA_DBSCAN == indexOfFileToRun)
            {
                int index = 1;
                foreach (var file in files)
                {
                    DataSetsGenerator.createDataSet();
                    List<string> paramsForAlgo = FileUtils.getParamsFromFile(file);
                    minPts = int.Parse(paramsForAlgo[minPtsIndex]);
                    eps = double.Parse(paramsForAlgo[epsIndex]);
                    dimentionsNum = int.Parse(paramsForAlgo[dimNumIndex]);

                    KPCA kpca = new KPCA(dimentionsNum);
                    managerLogic.RunDBSCANWithDimentionReduceAlgorithm(kpca.RunKPCA ,minPts, eps, fileName + index + ".txt");
                    index++;
                }
            }
            else if (TestsFilesIndex.RANDOM_PROJECTION_DBSCAN == indexOfFileToRun)
            {
                int index = 1;
                foreach (var file in files)
                {
                    DataSetsGenerator.createDataSet();
                    List<string> paramsForAlgo = FileUtils.getParamsFromFile(file);
                    minPts = int.Parse(paramsForAlgo[minPtsIndex]);
                    eps = double.Parse(paramsForAlgo[epsIndex]);
                    dimentionsNum = int.Parse(paramsForAlgo[dimNumIndex]);

                    RandomProjection randomProjection = new RandomProjection(dimentionsNum);
                    managerLogic.RunDBSCANWithDimentionReduceAlgorithm(randomProjection.RunRandomProjection, minPts, eps, fileName + index + ".txt");
                    index++;
                }
            }
        }

        #endregion


        #region Start Menu Functions


        private void executeChosenActionStartMenu(int chosenCommand)
        {
            switch (chosenCommand)
            {
                case 1:
                    ResizeImageChosen();
                    break;
                case 2:
                    AlreadyResizedChose();
                    break;
            }
        }


        #region Start Menu Command Execute

        private void ResizeImageChosen()
        {
            string facesFolderPath = getFolderPathFromUser();
            printRunning();
            managerLogic.ResizeImages(facesFolderPath);
        }

        private void AlreadyResizedChose()
        {
            string facesFolderPath = getFolderPathFromUser();
            FileUtils.CreateFilesList(facesFolderPath);
        }

        #endregion



        #region Start Menu Helpers

        private string getFolderPathFromUser()
        {
            Console.WriteLine(MenuStrings.InserFolderName);
            string facesFolderPath = Console.ReadLine();

            return facesFolderPath;
        }

        private bool isRightLimits(int chosenCommand, int minOption, int MaxOption)
        {
            //    if (chosenCommand >= minOption && chosenCommand <= MaxOption)
            //        return true;
            //    else return false;

            return chosenCommand >= minOption && chosenCommand <= MaxOption ? true : false;
        }

        #endregion


        #endregion

        private int presentManu(string title, List<string> menuOptions)
        {
            int minOption = 1;
            bool isInputChosen = false;
            bool isParsed = false;
            int chosenCommand = -1;

            while (!isInputChosen)
            {
                Console.WriteLine(title);
                Console.WriteLine(MenuStrings.Instructions);

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    Console.Write(i + 1 + ".");
                    Console.WriteLine(menuOptions[i]);
                }

                isParsed = int.TryParse(Console.ReadLine(), out chosenCommand);
                if (isParsed && isRightLimits(chosenCommand, minOption, menuOptions.Count))
                {
                    isInputChosen = true;
                }
            }

            return chosenCommand;
        }

        private int executePresentMenuOption(DelegateExecutePresentMenu presentMenue, string title, List<string> menuOptions)
        {
            int chosenCommand = -1;
            do
            {
                chosenCommand = presentMenue(title, menuOptions);

            } while (chosenCommand == -1);

            return chosenCommand;
        }

        private void createMenuesOptions()
        {
            startMenuOptions = createOptionsList("Resize Images", "Already Resized", MenuStrings.Exit);
            actionMenuOptions = createOptionsList("Regular DBSCAN", "DBSCAN with KPCA", "DBSCAN after Random Projection", MenuStrings.Exit);
        }

        private List<string> createOptionsList(params string[] optionList)
        {
            List<string> menuOptions = new List<string>();

            foreach (var option in optionList)
            {
                menuOptions.Add(option);
            }

            return menuOptions;
        }

        #region Helpers

        private Tuple<int, double> getDBSCANParamsFromUser()
        {
            int minPts;
            double eps;

            Console.WriteLine(MenuStrings.InsertMinPits);
            minPts = int.Parse(Console.ReadLine());

            Console.WriteLine(MenuStrings.InsertEps);
            eps = double.Parse(Console.ReadLine());

            return Tuple.Create(minPts, eps);
        }

        private int getNumberOfDimentionsFromUser()
        {
            Console.WriteLine(MenuStrings.InsertNumberOfDimentions);
            Console.WriteLine(MenuStrings.CurrentDimention + DataHolder.CurrentDimentionsNumber);

            return int.Parse(Console.ReadLine()); // the number of dimention the user insert
        }

        private string getFilePathOfLastAlgorithm(ReduseDimentionAlgorithmType algorithmType)
        {
            string fileName = string.Empty;
            if (algorithmType == ReduseDimentionAlgorithmType.KPCA)
            {
                fileName = KPCA.fileName;
            }
            else if (algorithmType == ReduseDimentionAlgorithmType.RANDOM_PROJECTION)
            {
                fileName = RandomProjection.fileName;
            }

            return fileName;
        }


        private bool isChooseExit(int chosenCommand, List<string> menuOptinsList)
        {
            return menuOptinsList[chosenCommand - 1].Equals(MenuStrings.Exit);
        }


        private void printRunning() { Console.WriteLine("running..."); }

        #endregion




        enum TestsFilesIndex
        {
            REGULAR_DBSCAN,
            KPCA_DBSCAN,
            RANDOM_PROJECTION_DBSCAN
        }

        enum ReduseDimentionAlgorithmType
        {
            KPCA,
            RANDOM_PROJECTION
        }

        private static class MenuStrings
        {
            public static string StartMenuTitle = "Start Menue";
            public static string Instructions = "Choose option by insert the option index number and then click 'enter'";
            public static string Exit = "Exit";
            public static string InserFolderName = "Insert folder name of the faces folder(e.g C:\\Users\\UserName\\Desktop\\faces) ";
            public static string ActionMenuTitle = "Action Menue";
            public static string ManualOption = "Run Manually";
            public static string TestsOptions = "Run with my Tests";

            public static string SubMenuTitle = "Sub Menu";
            public static string InsertEps = "Enter Epsilon value";
            public static string InsertMinPits = "Enter minimum number of points value";
            public static string InsertNumberOfDimentions = "Enter number of dimentions to reduce to";
            public static string CurrentDimention = "Current Dimention is: ";

            public static string RunReduceAlgoFirst = "Run Chosen Reduce Algoritm";
            public static string RunDBSCANonLastResult = "Run DBSCAN On Last Result";
            public static string Note = "Note: if you choose this option, you must have already run KPCA or RandomProjection (depand on your last choise)";
            public static string ResultFileLocation = "The Result in file 'finalResult.txt' Or 'testResult<testNumber>.txt'";


        }
    }
}
