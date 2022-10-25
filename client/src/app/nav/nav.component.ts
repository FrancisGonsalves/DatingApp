import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
@Component({
    selector: "app-nav",
    templateUrl: "./nav.component.html",
    styleUrls: ["./nav.component.less"]
})
export class NavComponent implements OnInit
{
    model: any = {}
    constructor(public accountService: AccountService) { }
    ngOnInit(): void {
        this.getCurrentUser();
    }
    login() {
        this.accountService.login(this.model).subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
        });
    }
    logout() {
        this.accountService.logout();
    }
    getCurrentUser() {
        this.accountService.currentUser$.subscribe((user: User) => {
        }, error => console.log(error));
    }
}