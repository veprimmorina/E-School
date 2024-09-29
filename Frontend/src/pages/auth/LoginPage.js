import React from "react";
import "../../assets/css/login.css"
import { Link } from 'react-router-dom';
import authLayout from "../../hoc/authLayout";

class LoginPage extends React.Component {
    constructor(props){
        super(props);

        this.state = {};
    }

    render(){
        return <>
         <form className="login-form">
                <div className="d-flex align-items-center justify-content-center my-4">
                    <h1 className="text-center fw-normal mb-0 me-3">Kyqu në E-Shkolla</h1>
                </div>
                
                <div className="d-flec justify-content-center text-lenter text-lg-start mt-4 pt-2">
                    <p className="small fw-bold mt-2 pt-1 mb-0 text-center">Platforma digjitale shkollore <a href="#!"
                        className="link-danger">E-Shkolla</a></p>
                </div>
                {/* <!-- Email input --> */}
                <div className="d-flex justify-content-center">
                <Link to="https://localhost:7116/Authentication/signin" type="button" className="btn btn-primary btn-lg mt-3">
                    <a class="btn btn-lg btn-google btn-block text-uppercase btn-outline text-white" href="https://localhost:7116/Authentication/signin"><img src="https://img.icons8.com/color/16/000000/google-logo.png"/> Google Sign in</a>
                    </Link>
                </div>
                <div className="d-flex justify-content-center align-items-center pt-4">
                    {/* <!-- Checkbox --> */}
                    <Link to="/reset-password" className="text-body">Forgot password?</Link>
                </div>
            </form>
        </>
    }
}

export default authLayout(LoginPage);