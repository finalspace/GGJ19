﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeUtils : Singleton<PipeUtils>
{
    /// <summary>
    /// check if adjacent blocks connect
    /// </summary>
    /// <returns>The connects.</returns>
    /// <param name="exitPoint">Exit point of current tile.</param>
    /// <param name="neighborType">pipe type id of the neighbor tile.</param>
    public bool Connects(int exitPoint, int neighborType)
    {
        bool[] accectedEntryPoints = GetConnectPointsByType(neighborType);
        return true;
    }

    public bool Connects(int exitPoint, PipeSection pipe)
    {
        int typeId = GetTypeId(pipe.m_Type, pipe.m_Orientation);
        return Connects(exitPoint, typeId);
    }

    public int GetTypeId(PipeSection.Type mainType, PipeSection.PipeRotation subType)
    {
        int typeId = 0;
        if (mainType == PipeSection.Type.Cross)
            typeId = 0;
        else if (mainType == PipeSection.Type.Straight)
        {
            if (subType == PipeSection.PipeRotation.NoRotation)
                typeId = 1;
            else typeId = 2;
        }
        else
        {
            if (subType == PipeSection.PipeRotation.NoRotation)
                typeId = 3;
            else if (subType == PipeSection.PipeRotation.Clockwise90)
                typeId = 4;
            else if (subType == PipeSection.PipeRotation.Clockwise180)
                typeId = 5;
            else typeId = 6;
        }

        return typeId;
    }

    public bool[] GetConnectPointsByType(PipeSection.Type mainType, PipeSection.PipeRotation subType)
    {
        int typeId = GetTypeId(mainType, subType);
        return GetConnectPointsByType(typeId);
    }

    public bool[] GetConnectPointsByType(int typeId)
    {
        bool[] connects;
        switch (typeId)
        {
            case 0:
                connects = new bool[4] { true, true, true, true };
                break;
            case 1:
                connects = new bool[4] { true, false, true, false };
                break;
            case 2:
                connects = new bool[4] { false, true, false, true };
                break;
            case 3:
                connects = new bool[4] { true, false, false, true };
                break;
            case 4:
                connects = new bool[4] { true, true, false, false };
                break;
            case 5:
                connects = new bool[4] { false, true, true, false };
                break;
            case 6:
                connects = new bool[4] { false, false, true, true };
                break;
            default:
                Debug.LogError("Invalid type Id");
                connects = new bool[4] { false, false, false, false };
                break;
        }

        return connects;
    }

    public int GetExitPoint(PipeSection pipe, int entryPoint)
    {
        int exitPoint = -1;
        int typeId = GetTypeId(pipe.m_Type, pipe.m_Orientation);
        switch (typeId)
        {
            case 0:
                exitPoint = (entryPoint + 2) % 4;
                break;
            case 1:
                if (entryPoint == 0)
                    exitPoint = 2;
                if (entryPoint == 2)
                    exitPoint = 0;
                break;
            case 2:
                if (entryPoint == 1)
                    exitPoint = 3;
                if (entryPoint == 3)
                    exitPoint = 1;
                break;
            case 3:
                if (entryPoint == 0)
                    exitPoint = 3;
                break;
            case 4:
                if (entryPoint == 0)
                    exitPoint = 1;
                break;
            case 5:
                if (entryPoint == 1)
                    exitPoint = 2;
                break;
            case 6:
                if (entryPoint == 2)
                    exitPoint = 3;
                break;
        }
        if (exitPoint == -1)
            Debug.LogError("Could not find exit point: " + typeId + " " + entryPoint);
        return exitPoint;
    }
}
