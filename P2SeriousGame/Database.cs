using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P2SeriousGame;
using P2SeriousGame.SQL;
using System.Data.SqlClient;

namespace P2SeriousGame
{
    public class Database
    {
        // not sure if used
        public Database() { }

        private long _elapsedSec;
        private float _secondsRound;
        private float _secondsTotal;
        private float _clickedTotal;

        private static int _totalLoss;
        
        public List<Round> roundList = new List<Round>();

        // Også defineret i administatorform...
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

        /*
        This method gets activated whenever the resetbutton is clicked.
        So the gameRoundWin is set to false before calling ResetGameToList();
        */
        public void ResetGameToListFromReset()
        {
            Pathfinding.gameRoundWin = false;
            ResetGameToList();
        }

        /*
        This method is either called whenever the round has finished
        or from ResetGameToListFromReset().
        */
        public void ResetGameToList()
        {
            ConvertSeconds();
            AddToTotal();
            RoundVariables();

            /*
            If nothing no clicks has happened is this round it is 
            possible to restart the round with no penalty.
            If there's clicks has happened this round the round will be added to the roundlist().
            */
            if (GameForm.hexClickedRound != 0)
            {
                Round round = new Round(GameForm.hexClickedRound, roundAverage, roundResult, _secondsRound);
                roundList.Add(round);
                /*
                Resets the amount of hex clicked, so it's ready to 
                count the number of clicks in the next round
                */
                GameForm.hexClickedRound = 0;
            }

            // Starts the stopwatch from 0
            stopwatchRound.Restart();
            // Increments the reset counter
            ResetCounter();
        }

        public void ExitGameToDatabase()
        {
            // Stops the stopwatch
            stopwatchRound.Stop();

            ConvertSeconds();
            AddToTotal();
            RoundVariables();

            if (GameForm.hexClickedRound != 0)
            {
                Round round = new Round(GameForm.hexClickedRound, roundAverage, roundResult, _secondsRound);
                roundList.Add(round);
            }

            if (_clickedTotal != 0)
            {
                // Adds the data from the lists to the database
                AddPersonToDatabase();
                AddRoundsToDatabase();
                AddSessionToDatabase();
            }
        }

        public void ConvertSeconds()
        {
            // Converts the time to seconds
            _elapsedSec = ElapsedSeconds();
            // Succesfully converts the long to float, ready for the database.
            _secondsRound = unchecked(_elapsedSec);
        }

        public void AddToTotal()
        {
            _secondsTotal += _secondsRound;
            _clickedTotal += GameForm.hexClickedRound;
        }

        public int roundResult;
        public float roundAverage;

        public void RoundVariables()
        {
            roundResult = WinOrLose();
            roundAverage = float.Parse(AverageClickPerMinute(GameForm.hexClickedRound, _secondsRound).ToString("n2"));
        }

        // Unique to WinOrLose
        private int _roundWin;
        private int _roundLoss;

        public int WinOrLose()
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

        public void AddPersonToDatabase()
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

        public void AddRoundsToDatabase()
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

        public void AddSessionToDatabase()
        {
            using (var context = new Entities())
            {
                context.Session.Add(new Session
                {
                    Clicks = _clickedTotal,
                    AVG_Clicks = AverageClickPerMinute(_clickedTotal, _secondsTotal),
                    Rounds = _resetCounter + 1,
                    Wins = Pathfinding.gameTotalWins,
                    Losses = _totalLoss,
                    Time_Used = _secondsTotal
                });
                context.SaveChanges();
            }
        }


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

        public long ElapsedSeconds()
        {
            return stopwatchRound.ElapsedMilliseconds / 1000;
        }
    }
}
