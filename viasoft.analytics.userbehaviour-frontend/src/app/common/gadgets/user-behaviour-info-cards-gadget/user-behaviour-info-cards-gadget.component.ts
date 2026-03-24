import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-behaviour-info-cards-gadget',
  templateUrl: './user-behaviour-info-cards-gadget.component.html',
  styleUrls: ['./user-behaviour-info-cards-gadget.component.scss']
})
export class UserBehaviourInfoCardsGadgetComponent implements OnInit {

  @Input()
  value: string;
  @Input()
  description: string;

  constructor() {}

  ngOnInit() {
  }
}
