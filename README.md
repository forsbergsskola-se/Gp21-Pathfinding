# Gp21-Pathfinding
My task was trying to create a pathfinding system for enemy AI that are mostly airborne, using A*.
When researching online how to create such a pathfinding system, I noticed that it's common to only use 2 axises(axes?) when creating a grid. Since my game is very much done in a 3D space, I ran into performance issues after adding a 3rd axis. I tried making the grid smaller, as well as optimize the code to not be as performance heavy.
One thing done to optimize the code in creating the grid was to change it from a series of if loops to instead use for loops.
 
In order to optimize performance I(with the help of my teacher), used the profiler in Unity to look at where performance was high in order to improve things. One thing we did to improve performance was turn the lists that I used into fields, and utilize the function .Clear. The reason for this was that by having the lists inside the methods, a new list was essentially made everytime the method was called(if I understood it correctly), and putting outside of the methods would go around this issue.
 
Another glaring issue I had when continuing to work on the problem was checking the path in Update. This is particularly performance heavy as the path is constantly checked and updated. I felt that using an alternative to using update would be problematic since both the player(target) and the enemy(startposition) could change often and 'rapidly', and since the movement in fluid as opposed to turn-based, update felt neccessary. 
 
In order to fix this, I did a number of things. First off, I change the size of the nodes, making sure there are less nodes to be checked. 
Then I made use of the coroutine function for a 'Timer' method that delays how often the pathfinding is updated. By making sure it's called frequently but not as frequently, I was finally starting to see improved results in the performance, and no longer have lags when my grid is larger than 15, 15, 15.
I also implemented a method that checks the closest node to location, and utilizes temporary nodes in some way. However, I did this with help from a programmer friend and I didn't fully understand what it does/how it works, but it also seemed to have a marginal benefit to performance.
 
I was also recommended to use priority queue to solve some of the performance issues caused by the lists but couldn't manage to figure out how to implement it.
While I have a functioning pathfinding algorithm, it's not optimized well enough, nor is it functioning enough to implement into the game a friend is making, which was the original intent. I did however learn a lot more about how the pathfinding algorithms work and will try to continue work on this after handing in the assignment.
