import { Component, OnInit } from '@angular/core';
import { DataService, IRequestData } from '../../../../services/data-service/data.service';

@Component({
  selector: 'app-colors-detail',
  imports: [],
  templateUrl: './colors-detail.component.html',
  styleUrl: './colors-detail.component.css'
})
export class ColorsDetailComponent implements OnInit {
  responseData: any = null;
  isLoading: boolean = true;
  errorMessage: string = '';

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
    const requestData: IRequestData<any> = {
      entity: 'colors',
      action: 'get',
      body: null,
      params: undefined
    };

    this.dataService.run(requestData).subscribe({
      next: (response) => {
        this.responseData = response;
        console.log('Dados recebidos:', this.responseData);
      },
      error: (error) => {
        this.errorMessage = 'Erro ao fazer requisição: ' + error.message;
        console.error('Erro na requisição:', error);
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

}
