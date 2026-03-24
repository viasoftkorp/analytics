import { Component, OnInit, OnDestroy, Injector } from '@angular/core';
import { Subscription } from 'rxjs';
import { GadgetBase } from '@viasoft/dashboard';
import { UserBehaviourOnlineAppsGadgetService } from './user-behaviour-online-apps-gadget.service';
import { switchMap, map } from 'rxjs/operators';
import { UsageSearchAppGadgetService } from '../usage-search-app-gadget/usage-search-app-gadget.service';

@Component({
  selector: 'vs-dynamic-component',
  templateUrl: './user-behaviour-online-apps-gadget.component.html',
  styleUrls: ['./user-behaviour-online-apps-gadget.component.scss']
})
export class UserBehaviourOnlineAppsGadgetComponent extends GadgetBase implements OnInit, OnDestroy {

  onlineApps: string;
  description = 'dashBoard.onlineApps.onlineAppsTitle';
  subs: Array<Subscription> = [];

  constructor(injector: Injector,
              private readonly userBehaviourOnlineAppsGadgetService: UserBehaviourOnlineAppsGadgetService,
              private readonly usageSearchService: UsageSearchAppGadgetService) {
    super(injector);
  }

  ngOnInit() {
    this.subs.push(this.userBehaviourOnlineAppsGadgetService.countAllOnlineApps().subscribe(
      (value) => {
        this.onlineApps = value.appsInUse.toString() + '/' + value.totalApps.toString();
      }
    ));

    this.subs.push(this.userBehaviourOnlineAppsGadgetService.advancedFilterChangedSubject.pipe(
      switchMap(
        () => this.userBehaviourOnlineAppsGadgetService.countAllOnlineApps()
      ),
      map(
        (value) => {
          this.onlineApps = value.appsInUse.toString() + '/' + value.totalApps.toString();
        }
      )
    ).subscribe());
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
    this.usageSearchService.renderAgain();
  }

  run(): void {
    this.initializeRunState(true);
    this.updateData(null);
  }
  stop(): void {
    this.setStopState(false);
  }
  updateProperties(updatedProperties: any): void {  }

  updateData(data: any[]): void {}

  preRun(): void {
    this.run();
  }

}
