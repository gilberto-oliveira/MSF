export class User {
    id: number;
    userName: string;
    passwordHash: string;
    firstName: string;
    lastName: string;
    email: string;
    token?: string;
    authenticated: boolean;
}

export interface UserChangePassword {
    email: string;
    currentPassword: string;
    newPassword: string;
}