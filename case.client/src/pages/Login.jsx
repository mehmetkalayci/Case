import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import api from "../utils/api";

export default function Login() {
  const handleSubmit = async (values, { setSubmitting, setFieldError }) => {
    try {
      const response = await api.post("/auth/login", values);
      
    } catch (error) {
      setFieldError("general", "Hatalı kullanıcı adı ya da şifre girildi.");
    }
    setSubmitting(false);
  };

  const validationSchema = Yup.object().shape({
    email: Yup.string().required("Eposta adresi girin"),
    password: Yup.string().required("Şifre girin"),
  });

  return (
    <>
      <div className="login-section">
        <div className="container">
          <div className="block">
            <div className="row justify-content-center">
              <div className="col-sm-12 col-lg-5 py-5">
                <Formik
                  initialValues={{ email: "", password: "" }}
                  validationSchema={validationSchema}
                  onSubmit={handleSubmit}
                >
                  {({ isSubmitting }) => (
                    <Form>
                      <div>
                        <label htmlFor="username">Eposta</label>
                        <Field
                          className="form-control"
                          type="email"
                          id="email"
                          name="email"
                        />
                        <ErrorMessage
                          name="email"
                          component="div"
                          style={{ color: "red" }}
                        />
                      </div>
                      <div>
                        <label htmlFor="password">Şifre</label>
                        <Field
                          className="form-control"
                          type="password"
                          id="password"
                          name="password"
                        />
                        <ErrorMessage
                          name="password"
                          component="div"
                          style={{ color: "red" }}
                        />
                      </div>
                      <div className="my-3">
                        <button
                          type="submit"
                          className="btn btn-primary-hover-outline"
                          disabled={isSubmitting}
                        >
                          Oturum Aç
                        </button>
                      </div>
                      <ErrorMessage
                        name="general"
                        component="div"
                        style={{ color: "red" }}
                      />
                    </Form>
                  )}
                </Formik>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
