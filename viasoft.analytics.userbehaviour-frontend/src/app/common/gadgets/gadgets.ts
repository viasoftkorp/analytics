import { VsGadgetDataSource } from '@viasoft/dashboard/lib/vs-dashboard.model';
import { environment } from 'src/environments/environment';

export const GADGETS: VsGadgetDataSource[] = [
  {
    componentType: 'UserBehaviourOnlineTenantsGadget',
    name: 'dashBoard.onlineTenants.onlineTenantsTitle',
    description: 'dashBoard.onlineTenants.onlineTenantsDescription',
    icon: `${environment.assetsUrl}user-tie-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'OnlineTenantsCount' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  },
  {
    componentType: 'UserBehaviouLicenseUsageByIdentifierGadget',
    name: 'dashBoard.licenseUsage.title',
    description: 'dashBoard.licenseUsage.description',
    icon: `${environment.assetsUrl}chart-area-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'LicenseUsage' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  },
  {
    componentType: 'UserBehaviourOnlineUsersGadget',
    name: 'dashBoard.onlineUsers.onlineUsersTitle',
    description: 'dashBoard.onlineUsers.onlineUsersDescription',
    icon: `${environment.assetsUrl}user-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'OnlineUsersCount' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  },
  {
    componentType: 'UserBehaviourOnlineAppsGadget',
    name: 'dashBoard.onlineApps.onlineAppsTitle',
    description: 'dashBoard.onlineApps.onlineAppsDescription',
    icon: `${environment.assetsUrl}server-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'OnlineAppsCount' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  },
  {
    componentType: 'UserBehaviourUsageSearchAppGadget',
    name: 'dashBoard.usageSearch.titles.app',
    description: 'dashBoard.usageSearch.descriptions.app',
    icon: `${environment.assetsUrl}chart-bar-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'UsageSearchApp' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  },
  {
    componentType: 'UserBehaviourUsageSearchDomainGadget',
    name: 'dashBoard.usageSearch.titles.domain',
    description: 'dashBoard.usageSearch.descriptions.domain',
    icon: `${environment.assetsUrl}chart-bar-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'UsageSearchDomain' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  },
  {
    componentType: 'UserBehaviourUsageSearchTenantGadget',
    name: 'dashBoard.usageSearch.titles.tenant',
    description: 'dashBoard.usageSearch.descriptions.tenant',
    icon: `${environment.assetsUrl}chart-bar-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'UsageSearchTenant' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  },
  /*
  {
    componentType: 'UsageSearchDatabaseGadget',
    name: 'dashBoard.usageSearch.titles.database',
    description: 'dashBoard.usageSearch.descriptions.database',
    icon: `${environment.assetsUrl}chart-bar-solid.svg`,
    instanceId: -1,
    permissionName: '',
    tags: [
      { facet: 'General', name: 'UsageSearchTenant' }
    ],
    config: {
      propertyPages: [
        {
          displayName: 'Run',
          groupId: 'run',
          properties: {}
        }
      ]
    },
    actions: [
      {
        name: 'dashBoard.add'
      }
    ]
  } */
];
