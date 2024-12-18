export class VehicleTypeResponseDTO {
    id: number = 0;
    name: string = '';
    passengersCapacity: number = 0;

    constructor(data: Partial<VehicleTypeResponseDTO> = { id: 0, name: "", passengersCapacity: 0 }) {
        Object.assign(this, data);
    }
}