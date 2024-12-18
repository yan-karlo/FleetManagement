import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { ColorResponseDTO } from '@models/color/color.response.dto';
import { FormTitleComponent } from '@shared/form/form-title/form-title.component';
import { SaveDeleteTryAgainButtonsBundleComponent } from '@shared/form/save-delete-try-again-buttons-bundle/save-delete-try-again-buttons-bundle.component';
import { SpinnerComponent } from '@shared/spinner/spinner.component';
import { FormAction } from '@app/Enums/FormAction.enum';
import { LoadingStatus } from '@app/Enums/loadingStatus.enum';
import { DataService, IRequestData } from '@services/data-service/data.service';
import { CheckLoading } from '@app/utils/CheckLoading';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-colors-form',
    imports: [
        ReactiveFormsModule,
        RouterModule,
        SpinnerComponent,
        FormTitleComponent,
        SaveDeleteTryAgainButtonsBundleComponent,
    ],
    templateUrl: './colors-form.component.html',
    styleUrl: './colors-form.component.css'
})
export class ColorsFormComponent implements OnInit {
    formTempMessageTitle = 'Colors Form:'
    color: ColorResponseDTO = new ColorResponseDTO();
    checkLoading: CheckLoading = new CheckLoading();
    colorForm!: FormGroup;
    formAction!: string;
    isDisabled: boolean = false;
    sendRequest: any = {
        'add': this.sendCreateRequest.bind(this),
        'update': this.sendUpdateRequest.bind(this),
        'delete': this.sendDeleteRequest.bind(this),
    };

    constructor(
        private readonly route: ActivatedRoute,
        private readonly router: Router,
        private readonly dataService: DataService,
        private readonly toastr: ToastrService
    ) { }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            var action = params.get('action') ?? "detail";
            var id = action === FormAction.add ? 0 : Number.parseInt(params.get('id') ?? '0');
            this.color.id = typeof id === 'number' ? id : -1;

            if (!(action in FormAction)) {
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse)
                throw new Error('A wrong endpoint was reached');
            }
            this.formAction = action;
        });

        this.colorForm = this.getNewFormGroup(this.color);

        if (this.color.id > 0) {
            this.sendGetByIdRequest();
        } else if (this.formAction === FormAction.add) {
            this.checkLoading.setLoadingStatus(LoadingStatus.Successful)
        } else {
            this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
        }


        this.colorForm.updateValueAndValidity();
    }

    onSubmit() {
        this.checkLoading.setLoadingStatus(LoadingStatus.RequestOngoing);
        this.sendRequest[this.formAction!]();
    }

    private getNewFormGroup(color: ColorResponseDTO): FormGroup {
        return new FormGroup({
            id: new FormControl(color.id),
            name: new FormControl(color.name, [Validators.required, Validators.maxLength(30), Validators.minLength(3)])
        });
    }

    private sendGetByIdRequest() {
        this.checkLoading.setLoadingStatus(LoadingStatus.ResponseOngoing);
        const requestData: IRequestData<null> = {
            entity: 'colors',
            action: 'get',
            body: null,
            params: `/byid/${this.color.id}`
        };
        this.dataService.run<null, ColorResponseDTO>(requestData).subscribe({
            next: (response) => {
                this.color = { ...response } as ColorResponseDTO;
                this.colorForm = this.getNewFormGroup(this.color);
            },
            error: (error) => {
                this.toastr.error('Error when trying to load the color from database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
                if (this.formAction === FormAction.delete || this.formAction === FormAction.detail) {
                    this.colorForm.get('name')?.disable();
                }
            }
        });
    }

    private sendCreateRequest() {
        Object.assign(this.color, this.colorForm.value);
        const requestData: IRequestData<ColorResponseDTO> = {
            entity: 'colors',
            action: 'post',
            // body: {"id" : 0, "name": this.color.name },
            body: null,
            params: `?colorName=${this.color.name}`,
        };
        this.dataService.run<ColorResponseDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The color name was saved.')
            },
            error: (error) => {
                this.toastr.error('Error when trying to save a new color name in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
                this.router.navigate(['/colors']);
            }
        });
    }

    private sendUpdateRequest() {
        Object.assign(this.color, this.colorForm.value);
        const requestData: IRequestData<ColorResponseDTO> = {
            entity: 'colors',
            action: 'put',
            body: this.color,
            params: '',
        };
        this.dataService.run<ColorResponseDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The color name was updated.')
                this.router.navigate(['/colors']);
            },
            error: (error) => {
                this.toastr.error('Error when trying to save the updated color name in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });

    }

    private sendDeleteRequest() {
        Object.assign(this.color, this.colorForm.value);
        const requestData: IRequestData<ColorResponseDTO> = {
            entity: 'colors',
            action: 'delete',
            body: null,
            params: `?id=${this.color.id}`,
        };
        this.dataService.run<ColorResponseDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The color name was deleted.')
                this.router.navigate(['/colors']);
            },
            error: (error) => {
                this.toastr.error('Error when trying to delete the actual color name in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });

    }

    updateLoadingStatus() {
        this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
    }

    onTryAgain() {
        this.sendGetByIdRequest();
    }
}
