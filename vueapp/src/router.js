import { createRouter, createWebHistory } from 'vue-router';
import Home from '@/components/HomePage.vue';

import Street from '@/components/Street/Street.vue';
import AddStreet from '@/components/Street/AddStreet.vue';
import EditStreet from '@/components/Street/EditStreet.vue';
import DeleteStreet from '@/components/Street/DeleteStreet.vue';

import Disrepair from '@/components/Disrepair/Disrepair.vue';
import AddDisrepair from '@/components/Disrepair/AddDisrepair.vue';
import EditDisrepair from '@/components/Disrepair/EditDisrepair.vue';
import DeleteDisrepair from '@/components/Disrepair/DeleteDisrepair.vue';

import Executor from '@/components/Executor/Executor.vue';
import AddExecutor from '@/components/Executor/AddExecutor.vue';
import EditExecutor from '@/components/Executor/EditExecutor.vue';
import DeleteExecutor from '@/components/Executor/DeleteExecutor.vue';

import Unit from '@/components/Unit/Unit.vue';
import AddUnit from '@/components/Unit/AddUnit.vue';
import EditUnit from '@/components/Unit/EditUnit.vue';
import DeleteUnit from '@/components/Unit/DeleteUnit.vue';

import ReceptionPoint from '@/components/ReceptionPoint/ReceptionPoint.vue';
import AddReceptionPoint from '@/components/ReceptionPoint/AddReceptionPoint.vue';
import EditReceptionPoint from '@/components/ReceptionPoint/EditReceptionPoint.vue';
import DeleteReceptionPoint from '@/components/ReceptionPoint/DeleteReceptionPoint.vue'; 

import Service from '@/components/Service/Service.vue';
import AddService from '@/components/Service/AddService.vue';
import EditService from '@/components/Service/EditService.vue';
import DeleteService from '@/components/Service/DeleteService.vue';

import Price from '@/components/Price/Price.vue';
import AddPrice from '@/components/Price/AddPrice.vue';
import EditPrice from '@/components/Price/EditPrice.vue';
import DeletePrice from '@/components/Price/DeletePrice.vue'; 

import Mode from '@/components/Mode/Mode.vue';
import AddMode from '@/components/Mode/AddMode.vue';
import EditMode from '@/components/Mode/EditMode.vue';
import DeleteMode from '@/components/Mode/DeleteMode.vue';

import AbonentMode from '@/components/AbonentMode/AbonentMode.vue';
import AddAbonentMode from '@/components/AbonentMode/AddAbonentMode.vue';
import EditAbonentMode from '@/components/AbonentMode/EditAbonentMode.vue';
import DeleteAbonentMode from '@/components/AbonentMode/DeleteAbonentMode.vue'; 

import NachislSumma from '@/components/NachislSumma/NachislSumma.vue';
import AddNachislSumma from '@/components/NachislSumma/AddNachislSumma.vue';
import EditNachislSumma from '@/components/NachislSumma/EditNachislSumma.vue';
import DeleteNachislSumma from '@/components/NachislSumma/DeleteNachislSumma.vue'; 

import Abonent from '@/components/Abonent/Abonent.vue';
import AddAbonent from '@/components/Abonent/AddAbonent.vue';
import EditAbonent from '@/components/Abonent/EditAbonent.vue';
import DeleteAbonent from '@/components/Abonent/DeleteAbonent.vue'; 

import Request from '@/components/Request/Request.vue';
import AddRequest from '@/components/Request/AddRequest.vue';
import EditRequest from '@/components/Request/EditRequest.vue';
import DeleteRequest from '@/components/Request/DeleteRequest.vue';

import Remain from '@/components/Remain/Remain.vue';
import AddRemain from '@/components/Remain/AddRemain.vue';
import EditRemain from '@/components/Remain/EditRemain.vue';
import DeleteRemain from '@/components/Remain/DeleteRemain.vue'; 

import PaySumma from '@/components/PaySumma/PaySumma.vue';
import AddPaySumma from '@/components/PaySumma/AddPaySumma.vue';
import EditPaySumma from '@/components/PaySumma/EditPaySumma.vue';
import DeletePaySumma from '@/components/PaySumma/DeletePaySumma.vue';

import UploadFile from '@/components/UploadFile.vue';

import Login from "./components/Authentication/Login.vue";
import Register from "./components/Authentication/Register.vue";
import Profile from "./components/Authentication/Profile.vue";
import BoardAdmin from "./components/Boards/BoardAdmin.vue"; 
import BoardUser from "./components/Boards/BoardUser.vue";

import PayServices from "./components/PayServices/PayServices.vue";
import NachislServices from "./components/NachislServices/NachislServices.vue";
import AbonentSearchHist from "./components/Abonent/AbonentSearchHist.vue"; 
import AbonentHist from "./components/Abonent/AbonentHist.vue";
import ActionsMenu from "./components/helpful/ActionsMenu.vue"; 

import RequestReportMonth from "./components/Request/RequestReportMonth.vue";
import PayNachislHist from "./components/NachislServices/PayNachislHist.vue"; 
import OSVEachAbonent from "./components/NachislServices/OSVEachAbonent.vue";


const routes = [
    {
        path: "/", component: Home,        
    },

    { path: "/street", component: Street },
    { path: "/street/add", component: AddStreet }, 
    { path: "/street/edit/:id", name: 'editstreet', component: EditStreet },
    { path: "/street/delete/:id", name: 'delstreet', component: DeleteStreet },

    { path: "/disrepair", component: Disrepair },
    { path: "/disrepair/add", component: AddDisrepair },
    { path: "/disrepair/edit/:id", name: 'editdisrepair', component: EditDisrepair },
    { path: "/disrepair/delete/:id", name: 'deldisrepair', component: DeleteDisrepair },

    { path: "/executor", component: Executor },
    { path: "/executor/add", component: AddExecutor },
    { path: "/executor/edit/:id", name: 'editexecutor', component: EditExecutor },
    { path: "/executor/delete/:id", name: 'delexecutor', component: DeleteExecutor },

    { path: "/unit", component: Unit },
    { path: "/unit/add", component: AddUnit },
    { path: "/unit/edit/:id", name: 'editunit', component: EditUnit },
    { path: "/unit/delete/:id", name: 'delunit', component: DeleteUnit },

    { path: "/receptionPoint", component: ReceptionPoint },
    { path: "/receptionPoint/add", component: AddReceptionPoint },
    { path: "/receptionPoint/edit/:id", name: 'editreceptionPoint', component: EditReceptionPoint },
    { path: "/receptionPoint/delete/:id", name: 'delreceptionPoint', component: DeleteReceptionPoint },

    { path: "/service", component: Service },
    { path: "/service/add", component: AddService },
    { path: "/service/edit/:id", name: 'editservice', component: EditService },
    { path: "/service/delete/:id", name: 'delservice', component: DeleteService },

    { path: "/price", component: Price },
    { path: "/price/add", component: AddPrice },
    { path: "/price/edit/:id", name: 'editprice', component: EditPrice },
    { path: "/price/delete/:id", name: 'delprice', component: DeletePrice },

    { path: "/mode", component: Mode },
    { path: "/mode/add", component: AddMode },
    { path: "/mode/edit/:id", name: 'editmode', component: EditMode },
    { path: "/mode/delete/:id", name: 'delmode', component: DeleteMode },

    { path: "/abonentMode", component: AbonentMode },
    { path: "/abonentMode/add", component: AddAbonentMode },
    { path: "/abonentMode/edit/:id", name: 'editabonentMode', component: EditAbonentMode },
    { path: "/abonentMode/delete/:id", name: 'delabonentMode', component: DeleteAbonentMode },

    { path: "/nachislSumma", component: NachislSumma },
    { path: "/nachislSumma/add", component: AddNachislSumma },
    { path: "/nachislSumma/add/:id", component: AddNachislSumma },
    { path: "/nachislSumma/edit/:id", name: 'editnachislSumma', component: EditNachislSumma },
    { path: "/nachislSumma/delete/:id", name: 'delnachislSumma', component: DeleteNachislSumma },

    { path: "/abonent", component: Abonent },
    { path: "/abonent/add", component: AddAbonent },
    { path: "/abonent/edit/:id", name: 'editabonent', component: EditAbonent },
    { path: "/abonent/delete/:id", name: 'delabonent', component: DeleteAbonent }, 
    
    { path: "/request", component: Request },
    { path: "/request/add", component: AddRequest },
    { path: "/request/edit/:id", name: 'editrequest', component: EditRequest },
    { path: "/request/delete/:id", name: 'delrequest', component: DeleteRequest }, 
    
    { path: "/remain", component: Remain },
    { path: "/remain/add", component: AddRemain },
    { path: "/remain/edit/:id", name: 'editremain', component: EditRemain },
    { path: "/remain/delete/:id", name: 'delremain', component: DeleteRemain }, 
    
    { path: "/paySumma", component: PaySumma },
    { path: "/paySumma/add", component: AddPaySumma },
    { path: "/paySumma/add/:id", component: AddPaySumma },
    { path: "/paySumma/edit/:id", name: 'editpaySumma', component: EditPaySumma },
    { path: "/paySumma/delete/:id", name: 'delpaySumma', component: DeletePaySumma },

    { path: "/uploadFile", component: UploadFile },

    { path: "/login", component: Login, },
    { path: "/register", component: Register, },
    { path: "/profile", component: Profile, },
    { path: "/admin", component: BoardAdmin, },    
    { path: "/user", component: BoardUser, },

    { path: "/pay/:id", name: 'abonentPay', component: PayServices, }, 
    { path: "/nachisl/:id", name: 'abonentCard', component: NachislServices },
    { path: "/abonentHist/:id", name: 'abonentHist', component: AbonentHist }, 
    { path: "/abonentSearchHist", component: AbonentSearchHist, }, 
    { path: "/actionsMenu", component: ActionsMenu, }, 

    { path: "/requestRepMonth", component: RequestReportMonth, },
    { path: "/payNachislHist/:id", name: 'abonentPayNachislHist', component: PayNachislHist, }, 
    { path: "/OSVEachAbonent", name: 'OSVEachAbonent', component: OSVEachAbonent, },

];

const router = createRouter({
    history: createWebHistory(),
    routes: routes
});

router.beforeEach((to, from, next) => {
    const publicPages = ['/login', '/register'];
    const authRequired = !publicPages.includes(to.path);
    const loggedIn = localStorage.getItem('user');
    if (authRequired && !loggedIn) {
        next('/login');
    } else {
        next();
    }
});

export default router;