import { Outlet } from "react-router";
import Sidebar from "../sideBar";
import styles from "./Admin.module.css"

export default function Admin () {
    return (
        <div className={`${styles.background} d-flex`} >
           <Sidebar/>
           <Outlet/>
        </div>
    )
}