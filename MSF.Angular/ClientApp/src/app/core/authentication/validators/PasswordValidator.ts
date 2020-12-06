import { AbstractControl } from '@angular/forms';

export class PasswordValidator {
    static Validate(control: AbstractControl) {
        const password = control.get('newPassword').value as string;
        if (password.length >= 5) {
            return null;
        } else {
            control.get('newPassword').setErrors({ PasswordValidator: true });
        }
    }
}
