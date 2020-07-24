export class User {
    id: number;
    userName: string;
    password: string;
    firstName: string;
    lastName: string;
    email: string;
    token?: string;
    refreshToken?: string;
    authenticated: boolean;
}

export interface UserChangePassword {
    email: string;
    currentPassword: string;
    newPassword: string;
}