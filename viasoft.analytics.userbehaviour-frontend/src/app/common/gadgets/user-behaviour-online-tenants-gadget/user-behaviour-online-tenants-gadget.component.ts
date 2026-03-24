import { Component, OnInit, Injector, OnDestroy } from '@angular/core';
import { GadgetBase } from '@viasoft/dashboard';
import { UserBehaviourOnlineTenantsGadgetService } from './user-behaviour-online-tenants-gadget.service';
import { Subscription } from 'rxjs';
import { UsageSearchAppGadgetService } from '../usage-search-app-gadget/usage-search-app-gadget.service';

@Component({
  selector: 'vs-dynamic-component',
  templateUrl: './user-behaviour-online-tenants-gadget.component.html',
  styleUrls: ['./user-behaviour-online-tenants-gadget.component.scss']
})
export class UserBehaviourOnlineTenantsGadgetComponent extends GadgetBase implements OnInit, OnDestroy {

  onlineClients: number;
  description = 'dashBoard.onlineTenants.onlineTenantsTitle';
  subs: Array<Subscription> = [];

  constructor(injector: Injector,
              private readonly userBehaviourOnlineClientsProxyService: UserBehaviourOnlineTenantsGadgetService,
              private readonly usageSearchAppGadgetService: UsageSearchAppGadgetService) {
    super(injector);
  }

  ngOnInit() {
    this.subs.push(this.userBehaviourOnlineClientsProxyService.countAllOnlineTenants().subscribe(
      (value) => {
        this.onlineClients = value.tenantCount;
      }
    ));

    setInterval(() => {
      this.subs.push(this.userBehaviourOnlineClientsProxyService.countAllOnlineTenants().subscribe(
        (value) => {
          this.onlineClients = value.tenantCount;
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
  updateProperties(updatedProperties: any): void {  }

  updateData(data: any[]): void {}

  preRun(): void {
    this.run();
  }

}
