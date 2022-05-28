using System.Collections.Generic;

namespace Minigames.SwordAndPistol.Scripts.Managers
{
    public static class DifficultyService 
    {
        static DifficultyService()
        {
            InitializeDictionary();
        }
    
        private static Dictionary<int, Difficulty> difficultyDictionary;
        private static Dictionary<DifficultyType, Difficulty> difficultyTypeDictionary;


        public static DifficultyType GetDifficultyType(int difficultyNumber)
        {
            var difficulty = GetDifficulty(difficultyNumber);
            return difficulty?.Type ?? DifficultyType.Normal;
        }

        public static string GetDifficultyName(int difficultyNumber)
        {
            var difficulty = GetDifficulty(difficultyNumber);
            return (difficulty != null) ? difficulty.Name : "UNDEFINED";
        }

        public static float GetDifficultySpawnPeriod(DifficultyType difficultyType)
        {
            var difficulty = GetDifficulty(difficultyType);
            return difficulty?.CubeSpawnPeriod ?? 0.8f;
        }
        
        private static Difficulty GetDifficulty(int difficultyNumber)
        {
            return difficultyDictionary.ContainsKey(difficultyNumber) ? difficultyDictionary[difficultyNumber] : null;
        }
        
        private static Difficulty GetDifficulty(DifficultyType difficultyType)
        {
            return difficultyTypeDictionary.ContainsKey(difficultyType) ? difficultyTypeDictionary[difficultyType] : null;
        }
        
        private static void InitializeDictionary()
        {
            difficultyDictionary = new Dictionary<int, Difficulty>()
            {
                {1, new Difficulty(DifficultyType.Easy, "EASY", 1.2f)}, 
                {2, new Difficulty(DifficultyType.Normal, "NORMAL", .8f)}, 
                {3, new Difficulty(DifficultyType.Hard, "HARD", .4f)}, 
            };

            difficultyTypeDictionary = new Dictionary<DifficultyType, Difficulty>();

            foreach (var difficulty in difficultyDictionary.Values)
            {
                difficultyTypeDictionary.Add(difficulty.Type, difficulty);
            }
        }

    }
}
