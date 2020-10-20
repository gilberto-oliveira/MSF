import { Operation } from './operation';

export class WorkCenterControl {
    id: number;
    workCenterId: number;
    userId: number;
    startDate: string;
    finalDate: string;
    operations: Operation[];
}
