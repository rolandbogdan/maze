import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-maze-generator',
  templateUrl: './maze-generator.component.html',
  styleUrls: ['./maze-generator.component.scss']
})
export class MazeGeneratorComponent {
  rows: number = 15;
  columns: number = 15;
  seed: string | undefined;
  generatedSeed: string | undefined;
  maze: any;

  constructor(private http: HttpClient) { }

  generateMaze() {
    this.rows < 10 ? this.rows = 10 : this.rows > 40 ? this.rows = 40 : this.rows = this.rows;
    this.columns < 10 ? this.columns = 10 : this.columns > 40 ? this.columns = 80 : this.columns = this.columns;
    let url = `https://localhost:7139/maze/generate?rows=${this.rows}&columns=${this.columns}`;
    if (this.seed) {
      url += `&seed=${this.seed}`;
    }
    this.http.get(url).subscribe((data: any) => {
      this.maze = data;
      this.generatedSeed = data.seed;
    });
  }

  solveMaze() {
    const url = `https://localhost:7139/maze/solve`;
    this.http.post(url, this.maze).subscribe((data: any) => {
      this.maze = data;
    }, error => {
      console.log("The maze could not be solved.");
    });
  }

  getCellClass(cell: any) {
    switch (cell.type) {
      case 0:
        return 'maze-cell wall';
      case 1:
        return 'maze-cell empty';
      case 2:
        return 'maze-cell start';
      case 3:
        return 'maze-cell end';
      case 4:
        return 'maze-cell path';
      default:
        return 'maze-cell';
    }
  }
}
