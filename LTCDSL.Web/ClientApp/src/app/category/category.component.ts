import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  public res: any;
  public listCategory: [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.post('https://localhost:44377/' + 'api/Categories/get-all',null).subscribe(result => {
      this.res = result;
      this.listCategory=this.res.data;
      console.log(this.listCategory);
    }, error => console.error(error));
  }

  ngOnInit() {
  }

}
