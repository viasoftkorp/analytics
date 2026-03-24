export const en = {
  userBehaviour: {
    licensingIdentifier: 'Tenant Id',
    user: 'User',
    appIdentifier: 'App Identifier',
    appName: 'App Name',
    bundleIdentifier: 'Bundle Identifier',
    bundleName: 'Bundle Name',
    softwareIdentifier: 'Software Identifier',
    softwareName: 'Software Name',
    softwareVersion: 'Software Version',
    hostName: 'Hostname',
    hostUser: 'Host User',
    localIpAddress: 'Local Ip Adress',
    language: 'Language',
    osInfo: 'OS Info',
    browserInfo: 'Browser Info',
    databaseName: 'Database Name',
    startTime: 'Login time',
    accessDuration: 'Access duration',
    lastUpdate: 'Last update',
    cnpj: 'Cnpj',
    additionallicense: 'Additional license',
    endTime: 'Logout time',
    durationInMinutes: 'Usage time in minutes',
    accountName: 'Account',
    domain: {
      domain: 'Domain',
      domains: {
        sales: 'Sales',
        purchases: 'Purchases',
        billing: 'Billing',
        financial: 'Financial',
        rma: 'Rma',
        logistics: 'Logistics',
        fiscal: 'Fiscal',
        accounting: 'Accounting',
        engineering: 'Engineering',
        production: 'Production',
        maintenance: 'Maintenance',
        qualityAssurance: 'Quality Assurance',
        humanResources: 'Human Resources',
        configurations: 'Configurations',
        development: 'Development',
        customized: 'Customized',
        licensing: 'Licensing',
        mobile: 'Mobile',
        reports: 'Reports'
      },
    },
  },
  dashBoard: {
    dashBoard: 'Dashboard',
    editLayout: 'Layout configurations',
    editPanels: 'Panels configurations',
    addGadget: 'Add widget',
    add: 'Add',
    save: 'Save',
    onlineTenants: {
      onlineTenantsTitle: 'Online tenants',
      onlineTenantsDescription: 'Counter with the number of tenants online.',
    },
    onlineUsers: {
      onlineUsersTitle: 'Online users',
      onlineUsersDescription: 'Counter with the number of users online.',
    },
    onlineApps: {
      onlineAppsTitle: 'Apps in use',
      onlineAppsDescription: 'Counter with the number of apps in use.',
    },
    usageSearchApp: {
      titles: {
        app: 'Usage search chart for apps',
        domain: 'Usage search chart for domains',
        tenant: 'Usage search chart for tenants'
      },
      descriptions: {
        app: 'Chart with the number of access of apps',
        domain: 'Chart with the number of access of domains',
        tenant: 'Chart with the number of access of tenants'
      },
      access: 'Accesses',
      groupings: {
        groupings: 'Groupings',
        app: {
          app: 'Applications',
          label: 'Top 10 most accessed applications'
        },
        domain: {
          domain: 'Domains',
          label: 'Top 10 most accessed domains'
        },
        tenant: {
          tenant: 'Tenants',
          label: 'Top 10 tenants who use the system most'
        }
      },
      waitForIndexProcess: 'Indexing in progress. Wait to be able to view historical data'
    },
    licenseUsage: {
      title: 'Online users',
      description: 'Flow of users online in one day.',
      hour: 'Hour',
      account: 'Account',
      interval: {
        title: 'Interval in minutes',
        fiveMinutes: '5 Minutes',
        tenMinutes: '10 Minutes',
        fifteenMinutes: '15 Minutes',
        thirtyMinutes: '30 Minutes',
        sixtyMinutes: '60 Minutes',
      },
      leftView: 'Previous time',
      rightView: 'Subsequent time',
      startView: 'Start of the day',
      endView: 'Current time',
    },
  },
  historic: {
    historic: 'Historic',
  },
  Gadget: {
    Filters: 'Filters',
    Settings: 'Settings',
    Remove: 'Remove',
  },
  Grid: {
    realTime: 'Use of Real Time Licenses',
  },
  HistoricIndex: {
    WaitForIndexProcess: 'Indexing in progress. Wait to be able to view historical data',
    Settings: {
      Title: 'History Indexing Configuration',
      Description: 'In case of inconsistencies in the usage history, you can re-index, rewriting all the historical data. This process takes time and, during the reindexing period, it will not be possible to view the usage history or the dashboard',
      ShouldReindexMessage: 'An error has occurred since the last reindexing. Click the button below to re-index',
      ErrorDuringReindexCall: 'Error when re-indexing, try again later',
      LastModificationTime: 'Last indexed date',
      Actions: {
        Reindex: 'Re-index',
        Reindexing: 'Indexing',
      }
    }
  }
};
