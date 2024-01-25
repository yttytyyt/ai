import random
from tkinter import *
from MazeCreator import Maze
from Player import Player
cell_size = 2
def draw_maze(canvas, maze_data, cell_size):
    canvas.delete('maze')  # Clear previous maze
    for y, row in enumerate(maze_data):
        for x, cell in enumerate(row):
            if cell == 'P':  # Path
                fill_color = 'white'
            elif cell == 'W':  # Wall
                fill_color = 'black'
            elif cell == 'S':  # Start position
                fill_color = 'green'
            elif cell == 'E':  # End position
                fill_color = 'red'
            else:
                fill_color = 'white'

            canvas.create_rectangle(x * cell_size, y * cell_size,
                                    (x + 1) * cell_size, (y + 1) * cell_size,
                                    fill=fill_color, tags='maze')

# other way:
# def draw_maze(canvas, maze_data, cell_size):
#     canvas.delete('maze')  # Clear previous maze
#     canvas.delete('fog')  # Clear previous fog
#
#     # Draw the maze
#     for y, row in enumerate(maze_data):
#         for x, cell in enumerate(row):
#             if cell == 'P':  # Path
#                 fill_color = 'white'
#             elif cell == 'W':  # Wall
#                 fill_color = 'black'
#             elif cell == 'S':  # Start position
#                 fill_color = 'green'
#             elif cell == 'E':  # End position
#                 fill_color = 'red'
#             else:
#                 fill_color = 'white'
#
#             canvas.create_rectangle(x * cell_size, y * cell_size,
#                                     (x + 1) * cell_size, (y + 1) * cell_size,
#                                     fill=fill_color, tags='maze')
#
#     # Draw fog of war
#     fog_color = "#A9A9A9"  # Dark gray
#     for y in range(maze.height):
#         for x in range(maze.width):
#             if abs(x - player.position[1]) > 1 or abs(y - player.position[0]) > 1:
#                 canvas.create_rectangle(x * cell_size, y * cell_size,
#                                         (x + 1) * cell_size, (y + 1) * cell_size,
#                                         fill=fog_color, tags='fog')

def on_key_press(event):
    global player, maze, cell_size
    reward = player.move(event.keysym)
    print(f"Reward: {reward}")
    draw_maze(maze_canvas, maze.maze, 20)  # Use maze data from Maze object
    player.draw(maze_canvas, 20)


def generate_and_draw_maze():
    global maze, player, cell_size
    maze = Maze(50, 50)
    maze.generate_maze()
    maze.set_start_and_end()  # Set start and end positions
    player = Player(maze.start_pos, maze)
    draw_maze(maze_canvas, maze.maze, 20)
    player.draw(maze_canvas, 20)
    difficulty = maze.calculate_difficulty()
    print("Maze Difficulty Rating:", difficulty)

cell_size = 20

window = Tk()
maze_canvas = Canvas(window, height=450, width=450, bg='white')
maze_canvas.pack(fill=BOTH, expand=True)

generate_and_draw_maze()  # Generate and draw maze at startup

window.bind("<KeyPress>", on_key_press)
window.focus_set()

window.mainloop()
