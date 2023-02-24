using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoundManager : MonoBehaviour {
    public bool debug = true;
    
    [Header("Round and Date Settings")]
    public int day_counter = 0;
    public int week_counter = 0;
    public int month_counter = 0;
    public int year_counter = 0;
    public Day current_day = Day.primas;

    [Header("Player Settings")]
    public List<Player> players = new();
    public Player current_player = null;

    private int player_index = 0;
    private readonly Keyboard keyboard = Keyboard.current;
    private UnitSelectionManager unit_manager;

    public void Start() {
        Debug.Log($"Current Player is {current_player}");
        unit_manager = GameObject
            .FindGameObjectWithTag("UnitSelectionManager")
            .GetComponent<UnitSelectionManager>();
    }

    public void AddResources() {
        foreach(var player in players) {
            player.HarvestFromBuildings();
        }
    }

    public void NextDay() {
        day_counter++;

        // After each day, players get resources from their buildings:
        AddResources();

        // Cycle through the 8 days of a week:
        if(current_day != Day.oktas) {
            current_day++;
        } else {
            current_day = Day.primas;
            
            // Cycle through the 4 weeks of a month:
            if(week_counter != 3) {
                week_counter++;
            } else {
                week_counter = 0;

                // Cycle through the 8 months of a year:
                if(month_counter != 7) {
                    month_counter++;
                } else {
                    month_counter = 0;
                    year_counter++;
                }
            }
        }

        Debug.Log($"Day {day_counter}; Current Day {current_day.ToString()}; " + 
                  $"Week {week_counter}; Month {month_counter}; Year {year_counter}");
    }

    public void NextPlayer() {
        if (current_player == null) {
            for(int index = 0; index < players.Count; index++) {
                // Start with the first non-AI player:
                if(!players[index].is_ai) {
                    player_index = index;
                    current_player = players[player_index];
                    current_player.LightAll(true);
                    //current_player.UpdateResourceDisplay();
                    current_player.LoadCamPos();
                    return;
                }
            }
            
        } else {
            // Make the last player invisible:
            current_player.LightAll(false);
            
            // Deselect all player units:
            current_player.DeselectAll();

            // Give back movement points to units:
            current_player.RefreshUnits();

            // Enable all cities of the player to build next round:
            current_player.RefreshCities();

            // Save the current camera position:
            current_player.SaveCamPos();
            
            // Cycle through player list:
            player_index++;
            if(player_index >= players.Count) {
                player_index = 0;
                
                // A day is over when all players got to turn:
                NextDay();
            }
            current_player = players[player_index];

            if(current_player.is_ai) {
                // Let the AI handle this players turn:
                current_player.MakeAITurn();

                // CAUTION: Using recursion here, the limit is about 100.000 for
                //          C#, which is alright i guess. Nevertheless this
                //          is just the easy way out of handling the AI. 
                //          A different (iterating) approach would be better...
                NextPlayer();
            } else {
                // Turn on visibility of all player belongings:
                current_player.LightAll(true);

                // Load the current camera position:
                current_player.LoadCamPos();

                // Replace current resource display:
                //current_player.UpdateResourceDisplay();
            }
        }
    }

    public void RegisterPlayer(Player player) {
        players.Add(player);
    }

    public static RoundManager Connect() {
        return GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>();
    }

    public void Update() {
        if(!debug) return;

        if(keyboard.nKey.wasPressedThisFrame) {
            NextPlayer();
        }
    }
}

public enum Day {
    primas,
    secundas,
    tertias,
    quartas,
    cintas,
    saxas,
    septos,
    oktas,
}
