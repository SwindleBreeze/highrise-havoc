using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Java.Util.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc.Source.Controllers
{
    public class TechTreeController
    {
        private Dictionary<String, Boolean> Technologies;

        public TechTreeController(Checkpoint checkpoint)
        {
            if(checkpoint != null)
            {
                // Technologies = checkpoint.Technologies;
            }
            else
            {
                Technologies = new Dictionary<String, Boolean>
                {
                    {"SpikeObstacle", false },
                    {"StrongObstacle", false},
                    {"StrongerObstacle", false },
                    {"CheaperObstacle", false },
                    {"StrongRifles", false },
                    {"StrongerRifles", false },
                    {"FastRifles", false },
                    {"FasterRifles", false },
                    {"BiggerMines", false },
                    {"TwoStepMines", false },
                    {"CheaperMines", false }
                };
            }
        }

        public String getObstacleUpgrdeDescription(String technology)
        {
            switch (technology)
            {
                case "SpikeObstacle":
                    return "Spike Obstacle: Obstacles deal 2x damage";
                case "StrongObstacle":
                    return "Strong Obstacle: Obstacles have 2x health";
                case "StrongerObstacle":
                    return "Stronger Obstacle: Obstacles have 3x health";
                case "CheaperObstacle":
                    return "Cheaper Obstacle: Obstacles cost 50% less";
                default:
                    return "";
            }
        }

        public String getRifleUpgradeDescription(String technology)
        {
            switch (technology)
            {
                case "StrongRifles":
                    return "Strong Rifles: Rifles deal 2x damage";
                case "StrongerRifles":
                    return "Stronger Rifles: Rifles deal 3x damage";
                case "FastRifles":
                    return "Fast Rifles: Rifles shoot 2x faster";
                case "FasterRifles":
                    return "Faster Rifles: Rifles shoot 3x faster";
                default:
                    return "";
            }
        }

        public String getMineUpgradeDescription(String technology)
        {
            switch (technology)
            {
                case "BiggerMines":
                    return "Bigger Mines: Mines deal 2x damage";
                case "TwoStepMines":
                    return "Two Step Mines: Mines explode twice";
                case "CheaperMines":
                    return "Cheaper Mines: Mines cost 50% less";
                default:
                    return "";
            }
        }

        public void UnlockTechnology(String technology)
        {
            Technologies[technology] = true;
        }

        public Boolean IsTechnologyUnlocked(String technology)
        {
            return Technologies.ContainsKey(technology) && Technologies[technology];
        }

        public void LoadTechTree()
        {
            try
            {
                String json = File.ReadAllText("Content/techtree.json");
                Technologies = JsonSerializer.Deserialize<Dictionary<String, Boolean>>(json);
            }
            catch (Exception e)
            {
                Logger.GetLogger(typeof(TechTreeController).Name).Log(Level.Warning, "Failed to load tech tree", (Java.Lang.Throwable)e);
            }
        }

        public void SaveTechTree()
        {
            try
            {
                String json = JsonSerializer.Serialize(Technologies);
                File.WriteAllText("Content/techtree.json", json);
            }
            catch (Exception e)
            {
                Logger.GetLogger(typeof(TechTreeController).Name).Log(Level.Warning, "Failed to save tech tree", (Java.Lang.Throwable)e);
            }
        }

        public void ResetTechTree()
        {
            Technologies.Clear();
        }
    }
}
