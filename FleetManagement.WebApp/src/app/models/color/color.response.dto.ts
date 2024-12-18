export class ColorResponseDTO {
    id: number = 0;
    name: string = "";

    constructor(data: Partial<ColorResponseDTO> = { id : 0, name: ''}) {
        Object.assign(this, data);
    }
}
