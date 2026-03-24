import { Component, OnInit } from '@angular/core';

import { UserBehaviourIndexService } from './user-behaviour-index.service';

@Component({
  selector: 'app-user-behaviour',
  templateUrl: './user-behaviour.component.html',
  styleUrls: ['./user-behaviour.component.scss']
})
export class UserBehaviourComponent implements OnInit {
  constructor(private userBehaviourIndexService: UserBehaviourIndexService) { }

  ngOnInit(): void {
    this.userBehaviourIndexService.start();
  }
}
