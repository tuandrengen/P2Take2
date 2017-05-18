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

namespace P2SeriousGame
{
    class Database
    {
        public Database() { }

        private long _elapsedSec;
        private float _secondsRound;
        private float _secondsTotal;
        private float _clickedTotal;

        private static int _totalLoss;

        public List<Round> roundList = new List<Round>();
        public List<Persons> personList = new List<Persons>();

        public string testName = "Dylan the creep";
        

        public void ResetGameToList()
        {
            ConvertSeconds();
            AddToTotal();
            RoundVariables();

            _totalLoss += 1;

            Persons person = new Persons(testName);
            personList.Add(person);

            Round round = new Round(GameForm.hexClickedRound, roundAverage, roundResult, _secondsRound);
            Console.WriteLine(roundAverage);
            roundList.Add(round);

            // Resets the amount of hex clicked
            GameForm.hexClickedRound = 0;
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
                Persons person = new Persons(testName);
                personList.Add(person);

                Round round = new Round(GameForm.hexClickedRound, roundAverage, roundResult, _secondsRound);
                roundList.Add(round);
            }

            if(_clickedTotal != 0)
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
            roundAverage = float.Parse(AverageClick(GameForm.hexClickedRound, _secondsRound).ToString("0.000"));
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
                return 0;
            }
        }

        public void AddPersonToDatabase()
        {
            using (var context = new Entities())
            {
                foreach (var row in personList)
                {
                    context.Person.Add(new Person
                    {
                        Name = row.Name // Error here when running
                    });
                }
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
                    AVG_Clicks = AverageClick(_clickedTotal, _secondsTotal),
                    Rounds = _resetCounter + 1,
                    Wins = Pathfinding.gameTotalWins,
                    Losses = _totalLoss,
                    Time_Used = _secondsTotal
                });
                context.SaveChanges();
            }
        }

        public void PrintData()
        {
            using (var context = new Entities())
            {
                foreach (var item in context.Rounds)
                {
                    Console.WriteLine(item.Id + " " + item);
                    Console.WriteLine(item.Clicks);
                    Console.WriteLine(item.AVG_Clicks);
                    Console.WriteLine(item.Win);
                    Console.WriteLine(item.Loss);
                    Console.WriteLine(item.Time_Used);
                }
            }
        }

        private float AverageClick(float hexClicked, float seconds)
        {
            return hexClicked / seconds;
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
