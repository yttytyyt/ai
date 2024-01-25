class Player:
    def __init__(self, start_pos, maze):
        self.position = start_pos
        self.maze = maze  # This is now the Maze object

    def draw(self, canvas, cell_size):
        canvas.delete('player')  # Clear previous player drawing
        y, x = self.position
        canvas.create_text(x * cell_size + cell_size // 2, y * cell_size + cell_size // 2, text="P", tags='player', fill='blue')

    def move(self, direction):
        y, x = self.position
        new_pos = (y, x)  # Default to current position

        if direction in ['Up', 'w']:
            new_pos = (y - 1, x)  # Move up
        elif direction in ['Down', 's']:
            new_pos = (y + 1, x)  # Move down
        elif direction in ['Left', 'a']:
            new_pos = (y, x - 1)  # Move left
        elif direction in ['Right', 'd']:
            new_pos = (y, x + 1)  # Move right

        reward = self.calculate_reward(new_pos)
        if reward != -10:  # Not a wall
            self.position = new_pos
        return reward

    def calculate_reward(self, new_pos):
        y, x = new_pos
        if y < 0 or y >= len(self.maze.maze) or x < 0 or x >= len(self.maze.maze[0]):
            return -10  # Wall or out of bounds
        elif self.maze.maze[y][x] == 'W':
            return -10  # Wall
        elif (y, x) == self.maze.end_pos:
            return 100  # Reached end
        else:
            return -1  # Regular move
