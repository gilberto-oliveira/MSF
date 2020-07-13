import { Subcategory } from './subcategory';

export class Category {
    id: number;
    code: string;
    description: string;
    subcategories: Subcategory[]
}