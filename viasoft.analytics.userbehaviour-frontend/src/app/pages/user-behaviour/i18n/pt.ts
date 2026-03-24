export const pt = {
  userBehaviour: {
    licensingIdentifier: 'Identificador do licenciamento',
    user: 'Usuário',
    appIdentifier: 'Identificador app',
    appName: 'Nome app',
    bundleIdentifier: 'Identificador pacote',
    bundleName: 'Nome pacote',
    softwareIdentifier: 'Identificador software',
    softwareName: 'Nome software',
    softwareVersion: 'Versão do software',
    hostName: 'Hostname',
    hostUser: 'Usuário do sistema operacional',
    localIpAddress: 'Endereço IP local',
    language: 'Linguagem',
    osInfo: 'Sistema Operacional',
    browserInfo: 'Browser info',
    databaseName: 'Nome Base de dados',
    startTime: 'Horário de acesso',
    accessDuration: 'Tempo em uso',
    lastUpdate: 'Última atualização',
    cnpj: 'Cnpj',
    additionallicense: 'Licença adicional',
    endTime: 'Horário de saída',
    durationInMinutes: 'Tempo de uso em minutos',
    accountName: 'Conta',
    domain: {
      domain: 'Domínio',
      domains: {
        sales: 'Vendas',
        purchases: 'Compras',
        billing: 'Faturamento',
        financial: 'Financeiro',
        rma: 'RMA',
        logistics: 'Logística',
        fiscal: 'Fiscal',
        accounting: 'Contábil',
        engineering: 'Engenharia',
        production: 'Produção',
        maintenance: 'Manutenção',
        qualityAssurance: 'Qualidade',
        humanResources: 'Recursos humanos',
        configurations: 'Configurações',
        development: 'Desenvolvimento',
        customized: 'Customizados',
        licensing: 'Licenciamento',
        mobile: 'Mobile',
        reports: 'Relatórios'
      },
    },
  },
  dashBoard: {
    dashBoard: 'Dashboard',
    editLayout: 'Abrir configurações de layout',
    editPanels: 'Abrir configurações de pranchetas',
    addGadget: 'Adicionar widget',
    add: 'Adicionar',
    save: 'Salvar',
    onlineTenants: {
      onlineTenantsTitle: 'Clientes online',
      onlineTenantsDescription: 'Contador com o número de clientes online.',
    },
    onlineUsers: {
      onlineUsersTitle: 'Usuários online',
      onlineUsersDescription: 'Contador com o número de usuários online.',
    },
    onlineApps: {
      onlineAppsTitle: 'Apps em uso',
      onlineAppsDescription: 'Contador com o número de apps em uso.',
    },
    usageSearch: {
      titles: {
        app: 'Gráfico de pesquisa de utilização de aplicativos',
        domain: 'Gráfico de pesquisa de utilização de domínios',
        tenant: 'Gráfico de pesquisa de utilização de clientes'
      },
      descriptions: {
        app: 'Gráfico com o número de acessos por aplicativo',
        domain: 'Gráfico com o número de acessos por domínio',
        tenant: 'Gráfico com o número de acessos por cliente'
      },
      access: 'Acessos',
      groupings: {
        groupings: 'Agrupamentos',
        app: {
          app: 'Aplicativos',
          label: 'Top 10 aplicativos mais acessados ({{dateInterval}})'
        },
        domain: {
          domain: 'Domínios',
          label: 'Top 10 domínios mais acessados ({{dateInterval}})'
        },
        tenant: {
          tenant: 'Cliente',
          label: 'Top 10 clientes que mais utilizam o sistema ({{dateInterval}})'
        }
      },
      waitForIndexProcess: 'Indexação em andamento. Aguarde para poder visualizar os dados de histórico'
    },
    licenseUsage: {
      title: 'Usuários online',
      description: 'Fluxo de usuários online em um dia.',
      hour: 'Horário',
      account: 'Conta',
      interval: {
        title: 'Intervalo em minutos',
        fiveMinutes: '5 Minutos',
        tenMinutes: '10 Minutos',
        fifteenMinutes: '15 Minutos',
        thirtyMinutes: '30 Minutos',
        sixtyMinutes: '60 Minutos',
      },
      leftView: 'Horário precedente',
      rightView: 'Horário subsequente',
      startView: 'Início do dia',
      endView: 'Horário atual',
    },
  },
  historic: {
    historic: 'Histórico',
  },
  Gadget: {
    Filters: 'Filtros',
    Settings: 'Configurações',
    Remove: 'Remover',
  },
  Grid: {
    realTime: 'Uso de Licenças em Tempo Real',
  },
  HistoricIndex: {
    WaitForIndexProcess: 'Indexação em andamento. Aguarde para poder visualizar os dados de histórico',
    Settings: {
      Title: 'Configuração de Indexação de Histórico',
      Description: 'Caso haja inconscistências no histórico de uso, você pode reindexar, reescrevendo todos os dados de histórico. Este processo leva tempo e, durante o período de reindexação, não será possível visualizar o histórico de uso ou o dashboard',
      ShouldReindexMessage: 'Ocorreu um erro desde a última reindexação. Clique no botão abaixo para reindexar',
      ErrorDuringReindexCall: 'Erro ao reindexar, tente novamente mais tarde',
      LastModificationTime: 'Data da última indexação',
      Actions: {
        Reindex: 'Reindexar',
        Reindexing: 'Indexando',
      }
    }
  }
};
