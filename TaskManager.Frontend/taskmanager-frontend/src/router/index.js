import { createRouter, createWebHistory } from 'vue-router';
import UserLogin from '../components/UserLogin.vue';
import TaskList from '../components/TaskList.vue';
import BaseLayout from '@/layouts/BaseLayout.vue';
import ProjectList from '@/components/ProjectList.vue';
import TimeTrackerList from '@/components/TimeTrackerList.vue';
import TaskListReport from '@/components/TaskListReport.vue';

const routes = [
  {
    path: '/',
    redirect: '/login',
  },
  {
    path: '/login',
    name: 'UserLogin',
    component: UserLogin,
  },
  {
    path: '/',
    component: BaseLayout,
    children: [
      {
        path: '/tasks',
        name: 'TaskList',
        component: TaskList,
        meta: { requiresAuth: true }
      },
      {
        path: '/projects',
        name: 'ProjectList',
        component: ProjectList,
        meta: { requiresAuth: true }
      },
      {
        path: '/timetrackers',
        name: 'TimeTrackerList',
        component: TimeTrackerList,
        meta: { requiresAuth: true }
      },
      {
        path: '/tasks/report',
        name: 'TaskListReport',
        component: TaskListReport,
        meta: { requiresAuth: true }
      },
    ]
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('token');
  if (to.matched.some(record => record.meta.requiresAuth) && !isAuthenticated) {
    next({ name: 'UserLogin' });
  } else {
    next();
  }
});

export default router;