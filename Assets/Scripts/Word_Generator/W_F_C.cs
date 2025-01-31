using System;
using System.Collections.Generic;
using UnityEngine;


//TODO: TP2 - Syntax - Consistency in naming convention
public class W_F_C : MonoBehaviour
{
    [SerializeField] private Grid grid;
    private bool stopWFC;
    //TODO - Fix - Wtf?
    //1. var model.now Overlappingodeimovt.N:3, width:48 height:48, periodicinput:true, pertedic:false,
    //    symmetry:8 ground:0 ):
    //2. model.Rum(random.Next, limit:#):
    //3.
    //Input - the training Image
    //N - HOW large of blocks(NoN) to sample from the Input as input patterns. (higher N leads to rising CPU
    //    and memory cost)
    //Width - The output width
    //Height - The output height
    //persedicingut-wther to sample the input across edges
    //periedic - whether the output should be sampled across edges to create edge-wrapping output
    //symetry - a value between 1..8. indicating how nany reflection and rotation symetries should be sampled
    //from the input

    public void StartWFC() 
    {
        grid.StartGrid();
        FirsNodeSelection();
        do
        {
            SearchLeastEntropy();

        } while (!stopWFC);
    }

    private void FirsNodeSelection() 
    {
        int x = UnityEngine.Random.Range(0,grid.grid.GetLength(0));
        int y = UnityEngine.Random.Range(0,grid.grid.GetLength(1));
        int z = UnityEngine.Random.Range(0,grid.grid.GetLength(2));

        Debug.Log(grid.grid[x, y, z].gridpos + new Vector3Int(1,0,1));
        CollapseSelection( ref grid.grid[x, y, z]);

    }
    private void UpdateEntropy(Node2D currentNode) 
    {

        List<RNode_Type> tempType = new List<RNode_Type>();
        foreach (RNode_Type item in Enum.GetValues(typeof(RNode_Type)))
        {
            tempType.Add(item);
        }

        int x = currentNode.gridpos.x;
        int y = currentNode.gridpos.y;
        int z = currentNode.gridpos.z;

        //Look Up
        if (currentNode.gridpos.z + 1 < grid.grid.GetLength(2))
        {
            if (grid.grid[x, y, z + 1].state == Node_States.Collapsed)
                return;

            grid.grid[x, y, z + 1].possible_Types = PossibilityOverlap(grid.grid[x, y, z + 1].possible_Types, currentNode.Possible_Neighbors[(int)currentNode.type]["Up"]);
        }

        //Look Down
        if (currentNode.gridpos.z - 1 >= 0)
        {
            if (grid.grid[x, y, z - 1].state == Node_States.Collapsed)
                return;

            grid.grid[x, y, z - 1].possible_Types = PossibilityOverlap(grid.grid[x, y, z - 1].possible_Types, currentNode.Possible_Neighbors[(int)currentNode.type]["Down"]);
        }

        //Look Right
        if (currentNode.gridpos.x + 1 < grid.grid.GetLength(0))
        {
            if (grid.grid[x + 1, y, z].state == Node_States.Collapsed)
                return;

            grid.grid[x + 1, y, z].possible_Types = PossibilityOverlap(grid.grid[x + 1, y, z].possible_Types, currentNode.Possible_Neighbors[(int)currentNode.type]["Right"]);
        }

        //Look Left
        if (currentNode.gridpos.x - 1 >= 0)
        {
            if (grid.grid[x - 1, y, z].state == Node_States.Collapsed)
                return;

            grid.grid[x - 1, y, z].possible_Types = PossibilityOverlap(grid.grid[x - 1, y, z].possible_Types, currentNode.Possible_Neighbors[(int)currentNode.type]["Left"]);
        }
    }
    private void SearchLeastEntropy() 
    {
        List<Node2D> sorted_Grid = SortGrid(grid.grid);
        if (sorted_Grid.Count == 0)
        {
            stopWFC = true;
            return;
        }
        int index = 0;
        for (int i = 0; i < sorted_Grid.Count; i++)
        {
            if (sorted_Grid[i].CompareTo(sorted_Grid[0]) != 0)
            {
                index = i;
                break;
            }
        }

        int rand = UnityEngine.Random.Range(0, index);
        //TODO: TP2 - Remove unused methods/variables
        //Debug.Log(sorted_Grid[0].possible_Types.Count);
        Vector3Int randpos = sorted_Grid[rand].gridpos;
        CollapseSelection(ref grid.grid[randpos.x, randpos.y, randpos.z]);
    }

    private void CollapseSelection( ref Node2D currentNode) 
    {
        List<RNode_Type> rNode_Types = new List<RNode_Type>();

        //TODO - Fix - Code is in Spanish or is trash code
        // Le asigna un valor aleatorio entre los valores que posee el nodo 
        int randType = (UnityEngine.Random.Range(0, currentNode.possible_Types.Count));
        RNode_Type selectedType = currentNode.possible_Types[randType];
        rNode_Types.Add(selectedType);


        currentNode.possible_Types = rNode_Types;
        currentNode.type = selectedType;
        currentNode.state = Node_States.Collapsed;

        UpdateEntropy(currentNode);
    }

    private List<RNode_Type> PossibilityOverlap(List<RNode_Type> list, RNode_Type[] rs_Type) 
    {
        List<RNode_Type> rs = new List<RNode_Type>();

        for (int i = 0; i < rs_Type.Length; i++)
        {
            if (list.Contains(rs_Type[i]))
            {
                rs.Add(rs_Type[i]);
            }
        }

        return rs;
    }

    public List<RNode_Type> GetNodePossibilities(Node2D node) 
    {
        List<RNode_Type> possible_Types = new List<RNode_Type>();
        foreach (RNode_Type item in Enum.GetValues(typeof(RNode_Type)))
        {
            possible_Types.Add(item);
        }

        int x = node.gridpos.x;
        int y = node.gridpos.y;
        int z = node.gridpos.z;

        //Look Up
        if (node.gridpos.z + 1 < grid.grid.GetLength(2))
        {
            Node2D up = grid.grid[x, y, z + 1];
            possible_Types = PossibilityOverlap(possible_Types, up.Possible_Neighbors[(int)up.type]["Down"]);
        }

        //Look Down
        if (node.gridpos.z - 1 >= 0)
        {
            Node2D down = grid.grid[x, y, z - 1];
            possible_Types = PossibilityOverlap(possible_Types, down.Possible_Neighbors[(int)down.type]["Up"]);
        }

        //Look Right
        if (node.gridpos.x + 1 < grid.grid.GetLength(0))
        {
            Node2D right = grid.grid[x + 1, y, z];
            possible_Types = PossibilityOverlap(possible_Types, right.Possible_Neighbors[(int)right.type]["Left"]);
        }

        //Look Left
        if (node.gridpos.x - 1 >= 0)
        {
            Node2D left = grid.grid[x - 1, y, z];
            possible_Types = PossibilityOverlap(possible_Types, left.Possible_Neighbors[(int)left.type]["Right"]);
        }

        return possible_Types;
    }

    private List<Node2D> SortGrid(Node2D[,,] grid_to_Sort) 
    {
        List<Node2D> output = new List<Node2D>();

        for (int x = 0; x < grid_to_Sort.GetLength(0); x++)
        {
            for (int y = 0; y < grid_to_Sort.GetLength(1); y++)
            {
                for (int z = 0; z < grid_to_Sort.GetLength(2); z++)
                {
                    if (grid_to_Sort[x, y, z].state == Node_States.UnCollapsed)
                    {
                        output.Add(grid_to_Sort[x, y, z]);
                    }
                }
            }
        }

        output.Sort();

        return output;
    }

}
