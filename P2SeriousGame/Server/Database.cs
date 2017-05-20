using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using P2SeriousGame.Server.SQL;
using System.Data.SqlClient;

namespace P2SeriousGame
{
    public class Database
    {        
        /// <summary>
        /// This method gets activated whenever the resetbutton is clicked.
        /// So the gameRoundWin is set to false before calling ResetGameToList();
        /// </summary>
        public void ResetGameToListFromReset()
        {
            Pathfinding.gameRoundWin = false;
            ResetGameToList();
        }

        public List<Round> roundList = new List<Round>();

        /// <summary>
        /// This method is either called whenever the round has finished
        /// or from ResetGameToListFromReset().
        /// </summary>
        public void ResetGameToList()
        {
            /// If no clicks has happened is this round it is 
            /// possible to restart the round with no penalty.
            /// If there's clicks has happened this round the round will be added to the roundlist().
            if (GameForm.hexClickedRound != 0)
            {
                ConvertSeconds();
                AddToTotal();
                RoundVariables();
                Round round = new Round(GameForm.hexClickedRound, _roundAverage, _roundResult, _secondsRound);
                roundList.Add(round);
                
                /// Increments the reset counter.
                ResetCounter();
            }
            /// Resets the amount of hex clicked, so it's ready to 
            /// count the number of clicks in the next round.
            GameForm.hexClickedRound = 0;
        }

        /// <summary>
        /// This method is in charge of closing off the game.
        /// </summary>
        public void ExitGameToDatabase()
        {
            stopwatchRound.Stop();

            if (GameForm.hexClickedRound != 0)
            {
                ConvertSeconds();
                AddToTotal();
                RoundVariables();

                Round round = new Round(GameForm.hexClickedRound, _roundAverage, _roundResult, _secondsRound);
                roundList.Add(round);
            }

            if (_clickedTotal != 0)
            {
                /// These three methods are in charge of distributing the data to the database.
                AddPersonToDatabase();
                AddRoundsToDatabase();
                AddSessionToDatabase();
            }
        }

        private long _elapsedSec;
        private float _secondsRound;

        /// <summary>
        /// Converts the time from milliseconds to seconds.
        /// Right after the variable gets converted from a long to a float,
        /// ready for the database.
        /// </summary>
        private void ConvertSeconds()
        {
            _elapsedSec = ElapsedSeconds();
            _secondsRound = unchecked(_elapsedSec);
            stopwatchRound.Restart();
        }

        private float _secondsTotal;
        private float _clickedTotal;

        /// <summary>
        /// Adds the amount of seconds and clicks from the round to the total amount
        /// These information is used in AddSessionToDatabase() - line ###.
        /// </summary>
        private void AddToTotal()
        {
            _secondsTotal += _secondsRound;
            _clickedTotal += GameForm.hexClickedRound;
        }

        private int _roundResult;
        private float _roundAverage;

        /// <summary>
        /// Gets the data about the game is a win or a loss.
        /// There after it calculates the average clicks per minute and formats it.
        /// This information is used to make a new instance of Round in ResetGameToList(),
        /// and ExitGameToDatabase().
        /// </summary>
        private void RoundVariables()
        {
            _roundResult = WinOrLose();
            _roundAverage = float.Parse(AverageClickPerMinute(GameForm.hexClickedRound, _secondsRound).ToString("n2"));
        }

        private int _roundWin;
        private int _roundLoss;
        private static int _totalLoss;

        /// <summary>
        /// This method returns either 1 or 0.
        /// These return values are used to determine if the round was a win or
        /// loss in the instantiation of Round in ResetGameToList(), and ExitGameToDatabase().
        /// </summary>
        /// <returns> Returns 1 or 0 returns>
        private int WinOrLose()
        {
            if (Pathfinding.gameRoundWin)
            {
                _roundLoss = 0;
                _roundWin = 1;
                return 1;
            }
            else
            {
                _roundLoss = 1;
                _roundWin = 0;
                _totalLoss++;
                return 0;
            }
        }

        /// <summary>
        /// This method is in charge of adding the player to the Person table in the database.
        /// While it's adding the person it may as well add the foreignkeys.
        /// </summary>
        private void AddPersonToDatabase()
        {
            using (var context = new Entities())
            {
                context.Person.Add(new Person
                {
                    Name = MainMenu.nameBox.Text
                });

                context.ForeignKeys.Add(new ForeignKeys
                {
                    PersonId = GetNextID(),
                    SessionId = GetNextID(),
                    RoundsId = GetNextID()
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// This method is in charge of adding the rounds to the Rounds table in the database.
        /// Since the rounds are listed in a List we use a foreach loop to add them to the Rounds table.
        /// </summary>
        private void AddRoundsToDatabase()
        {
            using (var context = new Entities())
            {
                foreach (var row in roundList)
                {
                    context.Rounds.Add(new Rounds
                    {
                        Clicks = row.NumberOfClicks,
                        AVG_Clicks = row.ClicksPerMinute,
                        Win = row.Win,
                        Loss = row.Loss,
                        Time_Used = row.TimeUsed
                    });
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// This method is in charge of adding the total data to the Session table in the database.
        /// </summary>
        private void AddSessionToDatabase()
        {
            using (var context = new Entities())
            {
                context.Session.Add(new Session
                {
                    Clicks = _clickedTotal,
                    AVG_Clicks = AverageClickPerMinute(_clickedTotal, _secondsTotal),
                    Rounds = _resetCounter,
                    Wins = Pathfinding.gameTotalWins,
                    Losses = _totalLoss,
                    Time_Used = _secondsTotal
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Calculates the average clicks per minute.
        /// </summary>
        /// <param name="hexClicked"></param>
        /// <param name="seconds"></param>
        /// <returns> Average clicks per minute </returns>
        private float AverageClickPerMinute(float hexClicked, float seconds)
        {
            return hexClicked / (seconds / 60);
        }

        private int _resetCounter;

        private void ResetCounter()
        {
            _resetCounter += 1;
        }

        Stopwatch stopwatchRound = new Stopwatch();

        public void StartStopwatch()
        {
            stopwatchRound.Start();
        }

        private long ElapsedSeconds()
        {
            return stopwatchRound.ElapsedMilliseconds / 1000;
        }

        /// Også defineret i administatorform...
        SqlConnection connection = new SqlConnection();
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
        {
            DataSource = "p2-avengers.database.windows.net",
            UserID = "tuandrengen",
            Password = "Aouiaom17",
            InitialCatalog = "p2-database"
        };

        public int GetNextID()
        {
            string query = "SELECT * FROM Person";

            using (connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable personTable = new DataTable();
                adapter.Fill(personTable);
                Console.WriteLine(personTable.Rows.Count);
                return personTable.Rows.Count + 1;
            }
        }
    }
}
