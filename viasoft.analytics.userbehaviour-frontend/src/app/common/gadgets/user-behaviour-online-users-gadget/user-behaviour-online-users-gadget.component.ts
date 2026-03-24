import { Component, OnInit, Injector, OnDestroy } from '@angular/core';
import { GadgetBase } from '@viasoft/dashboard';
import { UserBehaviourOnlineUsersGadgetService } from './user-behaviour-online-users-gadget.service';
import { Subscription } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { UsageSearchAppGadgetService } from '../usage-search-app-gadget/usage-search-app-gadget.service';

@Component({
  selector: 'vs-dynamic-component',
  templateUrl: './user-behaviour-online-users-gadget.component.html',
  styleUrls: ['./user-behaviour-online-users-gadget.component.scss']
})
export class UserBehaviourOnlineUsersGadgetComponent extends GadgetBase implements OnInit, OnDestroy {

  onlineUsers: number;
  description = 'dashBoard.onlineUsers.onlineUsersTitle';
  subs: Array<Subscription> = [];

  constructor(injector: Injector,
              private readonly userBehaviourOnlineUsersGadgetService: UserBehaviourOnlineUsersGadgetService,
              private readonly usageSearchAppGadgetService: UsageSearchAppGadgetService) {
    super(injector);
  }

  ngOnInit() {
    this.subs.push(this.userBehaviourOnlineUsersGadgetService.countAllOnlineUsers().subscribe(
      (value) => this.onlineUsers = value.onlineUserCount
    ));

    this.subs.push(this.userBehaviourOnlineUsersGadgetService.advancedFilterChangedSubject.pipe(
      switchMap(
        () => this.userBehaviourOnlineUsersGadgetService.countAllOnlineUsers()
      ),
      map(
        (result) => this.onlineUsers = result.onlineUserCount)
    ).subscribe());

    setInterval(() => {
      this.subs.push(this.userBehaviourOnlineUsersGadgetService.countAllOnlineUsers().subscribe(
        (value) => {
          this.onlineUsers = value.onlineUserCount;
          this.subs.pop().unsubscribe();
        }
      ));
    }, 60000);
  }

  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
    this.usageSearchAppGadgetService.renderAgain();
  }

  run(): void {
    this.initializeRunState(true);
    this.updateData(null);
  }
  stop(): void {
    this.setStopState(false);
  }
  updateProperties(updatedProperties: any): void {

  }

  updateData(data: any[]): void { }

  preRun(): void {
    this.run();
  }

}
