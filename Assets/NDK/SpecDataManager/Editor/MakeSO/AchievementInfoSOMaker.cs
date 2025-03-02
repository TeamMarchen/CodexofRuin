// using System;
// using System.IO;
// using UnityEngine;
//
// public class AchievementInfoSOMaker : SpecDataSOMaker
// {
//     public override ScriptableObject ProcessData(string[] data, string fullPath)
//     {
//         AchievementInfoSO achievementInfoSo = GetSO<AchievementInfoSO>(GetAssetPath(Path.Combine(fullPath, data[0])));
//
//         achievementInfoSo.id = data[0];
//         achievementInfoSo.Title = data[2];
//         achievementInfoSo.Grade = int.Parse(data[3]);
//         achievementInfoSo.Description = data[4];
//         achievementInfoSo.AchievementType = Enum.Parse<Enums.AchievementType>(data[5]);
//         achievementInfoSo.Count = int.Parse(data[6]);
//         achievementInfoSo.DiaAmount = int.Parse(data[7]);
//
//         return achievementInfoSo;
//     }
// }
