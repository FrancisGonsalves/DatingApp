import { Component, Input, Output, EventEmitter } from "@angular/core";
import { AccountService } from "../_services/account.service";
@Component({
    selector: "app-register",
    templateUrl: "./register.component.html",
    styleUrls: ["./register.component.css"]
})
export class RegisterComponent
{
    model: any = {};
    @Output() cancelRegister = new EventEmitter();
    constructor(private accountService: AccountService) {}
    register() {
        this.accountService.register(this.model).subscribe(res => { 
            console.log(res);
            this.cancel();
        }, error => console.log(error));
    }
    cancel() {
        this.cancelRegister.emit(false);
    }
}