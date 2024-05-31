import { createRouter, createWebHistory } from 'vue-router'
import Login from '@/views/Login.vue'
import Home from '@/views/Home.vue'
import Register from '@/views/Register.vue'
import Semesters from '@/views/Semesters.vue'
import Subjects from '@/views/Subjects.vue'
import SubjectStats from '@/views/SubjectStats.vue'
import SubjectGrade from '@/views/SubjectGrade.vue'
import Homeworks from '@/views/Homeworks.vue'
import HomeworkStats from '@/views/HomeworkStats.vue'
import HomeworkGrade from '@/views/HomeworkGrade.vue'
// import Categories from '@/views/todo/categories/Categories.vue'
// import CreateCategory from '@/views/todo/categories/CreateCategory.vue'
// import EditCategory from '@/views/todo/categories/EditCategory.vue'
// import Priorities from '@/views/todo/priorities/Priorities.vue'
// import CreatePriority from '@/views/todo/priorities/CreatePriority.vue'
// import EditPriority from '@/views/todo/priorities/EditPriority.vue'
// import Tasks from '@/views/todo/tasks/Tasks.vue'
// import CreateTask from '@/views/todo/tasks/CreateTask.vue'
// import EditTask from '@/views/todo/tasks/EditTask.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/", name: "Home", component: Home
    },
    {
      path: "/login", name: "Login", component: Login
    },
    {
      path: "/register", name: "Register", component: Register
    },
    {
      path: "/semesters", name: "Semester", component: Semesters
    },
    {
      path: "/subjects/:id", name: "Subjects", component: Subjects
    },
    {
      path: "/subjectStats/:id", name: "SubjectStats", component: SubjectStats
    },
    {
      path: "/subjectGrade/:id", name: "SubjectGrade", component: SubjectGrade
    },
    {
      path: "/homeworks/:id", name: "Homeworks", component: Homeworks
    },
    {
      path: "/homeworkstats/:id", name: "HomeworkStats", component: HomeworkStats
    },
    {
      path: "/homeworkgrade/:id", name: "HomeworkGrade", component: HomeworkGrade
    },
    // {
    //   path: "/categories", name: "Categories", component: Categories
    // },
    // {
    //   path: "/categories/create", name: "CreateCategory", component: CreateCategory
    // },
    // {
    //   path: "/categories/edit/:id", name: "EditCategory", component: EditCategory
    // },
    // {
    //   path: "/priorities", name: "Priorities", component: Priorities
    // },
    // {
    //   path: "/priorities/create", name: "CreatePriority", component: CreatePriority
    // },
    // {
    //   path: "/priorities/edit/:id", name: "EditPriority", component: EditPriority
    // },
    // {
    //   path: "/tasks", name: "Tasks", component: Tasks
    // },
    // {
    //   path: "/tasks/create", name: "CreateTask", component: CreateTask
    // },
    // {
    //   path: "/tasks/edit/:id", name: "EditTask", component: EditTask
    // },
  ]
})

export default router
