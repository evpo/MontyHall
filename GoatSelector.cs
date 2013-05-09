/********************************************************************************************
Goat Selector

DESCRIPTION: 
Monty Hall problem (http://en.wikipedia.org/wiki/Monty_Hall_problem) simulation. 
The program runs 10000 games for switch the door and stick.

OUTPUT: 
Rates of success for the switch the door and stick strategies.

HOW TO RUN:
Open Visual Studio Command Prompt
Execute:
csc.exe GoatSelector.cs
GoatSelector.exe

LINK:
See this page for more details:
http://evpo.wordpress.com/2008/06/25/car-and-two-goats/

Evgeny Pokhilko 2013
*********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Summary description for Class1
/// </summary>
public class GoatSelector
{
    private const int _games = 10000;

    private int _successWhenSwitching = 0;
    private int _successWhenStaying = 0;
    private int[] _validation = new int[] { 0, 0, 0 };

    private bool[][] _rooms = new bool[][]
    {
        new bool[]{false, false, true}, 
        new bool[]{false, true, false},
        new bool[]{true, false, false}
    };

    private Random _random = new Random();

    private void Validate()
    {
        for (int i = 0; i < _games; i++)
        {
            _validation[GetRandom(0, 1, 2)]++;
        }
    }

    public int GetRandom(params int[] options)
    {
        return options[_random.Next(0, options.Length)];
    }

    public int[] SubtractSets(int[] subtrahend, int[] predicate)
    {
        List<int> retVal = new List<int>();
        for (int i = 0; i < subtrahend.Length; i++)
        {
            bool found = false;
            for (int x = 0; x < predicate.Length; x++)
            {
                if (subtrahend[i] == predicate[x])
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                retVal.Add(subtrahend[i]);
            }
        }
        return retVal.ToArray();
    }

    public void Assert(bool result, string message)
    {
        if (!result)
        {
            throw new ApplicationException(message);
        }
    }

    private double GetPercents(int num, int all)
    {
        return ((double)num / (double)all) * 100;
    }

    public static void Main(string[] args)
    {
        GoatSelector selector = new GoatSelector();
        selector.Run();
    }

    public void Run()
    {
        Validate();
        RunGame(true);
        RunGame(false);

        Console.WriteLine("Total games: " + _games.ToString());

        Console.WriteLine(
                String.Format(
                    "Validation: 0: {0}%, 1: {1}%, 2: {2}%", 
                    GetPercents(_validation[0], _games),
                    GetPercents(_validation[1], _games),
                    GetPercents(_validation[2], _games)
                    )
            );
        Console.WriteLine(
            String.Format("When switching: {0} %", GetPercents(_successWhenSwitching, _games))
            );
        Console.WriteLine(
            String.Format("When staying: {0} %", GetPercents(_successWhenStaying, _games))
            );
        Console.Read();
    }

    public void RunGame(bool switching)
    {
        for (int i = 0; i < _games; i++)
        {
            bool[] rooms = GetRooms();
            int firstRoom = GetRandom(0, 1, 2);

            int[] remainingRooms = SubtractSets(new int[] { 0, 1, 2 }, new int[] { firstRoom });
            Assert(remainingRooms.Length == 2, "there must be two rooms left");

            int roomWithCar = -1;
            if (rooms[remainingRooms[0]])
            {
                roomWithCar = remainingRooms[0];
            }

            if (rooms[remainingRooms[1]])
            {
                roomWithCar = remainingRooms[1];
            }

            int[] roomsToOpen = SubtractSets(remainingRooms, new int[] { roomWithCar });
            int openedRoom = -1;
            if (roomsToOpen.Length == 1)
            {
                openedRoom = roomsToOpen[0];
            }
            else
            {
                openedRoom = GetRandom(roomsToOpen[0], roomsToOpen[1]);
            }

            if (switching)
            {
                if (rooms[SubtractSets(new int[] { 0, 1, 2 }, new int[] { firstRoom, openedRoom })[0]])
                {
                    _successWhenSwitching++;
                }
            }
            else
            {
                if (rooms[firstRoom])
                {
                    _successWhenStaying++;
                }
            }
        }
    }

    public bool[] GetRooms()
    {
        return _rooms[GetRandom(0, 1, 2)];
    }

    
}
