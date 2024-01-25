import random
import queue
import sys


class Maze:
    def __init__(self, width, height):
        # sys.setrecursionlimit(30000)
        self.width = width + (width + 1) % 2
        self.height = height + (height + 1) % 2
        self.maze = self._initialize_maze()
        self.start_pos = None
        self.end_pos = None
        self.max_wall_length = max(self.width, self.height) // 5 + 1


    def _initialize_maze(self):
        return [['W' for _ in range(self.width)] for _ in range(self.height)]

    def generate_maze(self):
        # Start from a random edge cell
        edges = [(0, random.randint(1, self.width - 2)),
                 (self.height - 1, random.randint(1, self.width - 2)),
                 (random.randint(1, self.height - 2), 0),
                 (random.randint(1, self.height - 2), self.width - 1)]
        start_y, start_x = random.choice(edges)
        self._recursive_backtracking(start_y, start_x)

    def _recursive_backtracking(self, y, x):
        self.maze[y][x] = 'P'
        directions = [(0, 2), (2, 0), (0, -2), (-2, 0)]
        random.shuffle(directions)

        for dy, dx in directions:
            ny, nx = y + dy, x + dx
            if 0 <= ny < self.height and 0 <= nx < self.width and self.maze[ny][nx] == 'W':
                if self._is_valid_position(ny, nx):
                    self._remove_wall(y, x, ny, nx)
                    self._recursive_backtracking(ny, nx)

    def _is_valid_position(self, y, x):
        # Allow the border cells to be part of the maze
        if 0 <= y < self.height and 0 <= x < self.width and self.maze[y][x] == 'W':
            path_neighbors = sum(
                0 <= y + dy < self.height and 0 <= x + dx < self.width and
                self.maze[y + dy][x + dx] == 'P'
                for dy, dx in [(0, 1), (1, 0), (0, -1), (-1, 0)]
            )
            # Check for corner cells which should never be path cells
            is_corner = (y == 0 and x == 0) or \
                        (y == 0 and x == self.width - 1) or \
                        (y == self.height - 1 and x == 0) or \
                        (y == self.height - 1 and x == self.width - 1)
            return path_neighbors == 0 and not is_corner
        return False

    def add_border(self):
        for x in range(self.width):
            self.maze[0][x] = 'W'
            self.maze[self.height - 1][x] = 'W'
        for y in range(self.height):
            self.maze[y][0] = 'W'
            self.maze[y][self.width - 1] = 'W'
        # Ensure the start and end positions are open
        self.maze[self.start_pos[0]][self.start_pos[1]] = 'S'
        self.maze[self.end_pos[0]][self.end_pos[1]] = 'E'

    def _remove_wall(self, y1, x1, y2, x2):
        # Converts the wall between two cells into a path
        wall_y, wall_x = (y1 + y2) // 2, (x1 + x2) // 2
        self.maze[wall_y][wall_x] = 'P'
        self.maze[y2][x2] = 'P'  # Ensure the destination cell becomes a path as well

    def _check_boundary_constraint(self, y, x):
        if y == 0 or y == self.height - 1 or x == 0 or x == self.width - 1:
            wall_length = 1
            for dy, dx in [(0, 1), (1, 0), (0, -1), (-1, 0)]:
                ny, nx = y + dy, x + dx
                while 0 <= ny < self.height and 0 <= nx < self.width and self.maze[ny][nx] == 'W':
                    wall_length += 1
                    ny += dy
                    nx += dx
            return wall_length < self.max_wall_length
        return True

    def set_start_and_end(self):
        # Set start position on the upper border, not in the corner
        self.start_pos = (1, random.randint(1, self.width - 3))
        # Set end position on the lower border, not in the corner
        self.end_pos = (self.height - 2, random.randint(1, self.width - 3))
        # Mark them on the maze
        self.maze[self.start_pos[0]][self.start_pos[1]] = 'S'
        self.maze[self.end_pos[0]][self.end_pos[1]] = 'E'

    def calculate_path_length(self):
        if not self.start_pos or not self.end_pos:
            return None

        visited = [[False for _ in range(self.width)] for _ in range(self.height)]
        q = queue.Queue()
        q.put((self.start_pos, 0))

        while not q.empty():
            (y, x), dist = q.get()
            if (y, x) == self.end_pos:
                return dist

            if not visited[y][x]:
                visited[y][x] = True
                for dy, dx in [(0, 1), (1, 0), (0, -1), (-1, 0)]:
                    ny, nx = y + dy, x + dx
                    if 0 <= ny < self.height and 0 <= nx < self.width:
                        if self.maze[ny][nx] in ['P', 'E'] and not visited[ny][nx]:
                            q.put(((ny, nx), dist + 1))

        return None

    def calculate_dead_ends(self):
        dead_end_count = 0
        for y in range(self.height):
            for x in range(self.width):
                if self.maze[y][x] == 'P' and self._is_dead_end(y, x):
                    dead_end_count += 1
        return dead_end_count

    def _is_dead_end(self, y, x):
        open_neighbors = 0
        for dy, dx in [(0, 1), (1, 0), (0, -1), (-1, 0)]:
            ny, nx = y + dy, x + dx
            if 0 <= ny < self.height and 0 <= nx < self.width:
                if self.maze[ny][nx] in ['P', 'S', 'E']:
                    open_neighbors += 1
        return open_neighbors == 1

    def calculate_branches(self):
        branch_count = 0
        for y in range(self.height):
            for x in range(self.width):
                if self.maze[y][x] == 'P' and self._is_branch(y, x):
                    branch_count += 1
        return branch_count

    def _is_branch(self, y, x):
        open_neighbors = 0
        for dy, dx in [(0, 1), (1, 0), (0, -1), (-1, 0)]:
            ny, nx = y + dy, x + dx
            if 0 <= ny < self.height and 0 <= nx < self.width:
                if self.maze[ny][nx] in ['P', 'S', 'E']:
                    open_neighbors += 1
        return open_neighbors > 2

    def calculate_difficulty(self):
        path_length = self.calculate_path_length()
        dead_ends = self.calculate_dead_ends()
        branches = self.calculate_branches()

        # Default values if any metric is None
        default_path_length = 0
        default_dead_ends = 0
        default_branches = 0

        # Using default values if metrics are None
        path_length = path_length if path_length is not None else default_path_length
        dead_ends = dead_ends if dead_ends is not None else default_dead_ends
        branches = branches if branches is not None else default_branches

        weight_path_length = 1
        weight_dead_ends = 1
        weight_branches = 1

        difficulty_rating = (weight_path_length * path_length) + \
                            (weight_dead_ends * dead_ends) + \
                            (weight_branches * branches)
        return difficulty_rating


# Test the Maze class
if __name__ == "__main__":
    highestDif = 0
    c = 1000
    i = 0
    maze = Maze(20, 20)
    maze.generate_maze()
    maze.set_start_and_end()
    difficulty = maze.calculate_difficulty()
    print("Maze Difficulty Rating:", difficulty)
    for row in maze.maze:
        print(' '.join(row))



