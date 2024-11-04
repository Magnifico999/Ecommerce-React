import { Elements } from "@stripe/react-stripe-js";
import CheckoutPage from "./CheckoutPage";
import { loadStripe } from "@stripe/stripe-js";
import { useAppDispatch } from "../../app/store/configureStore";
import { useEffect, useState } from "react";
import agent from "../../app/api/agent";
import { setBasket } from "../basket/basketSlice";
import LoadingComponent from "../../app/layout/LoadingComponent";

const stripePromise = loadStripe('pk_test_51O9hh6H0KsejqWMFV7cqJtG2m9YgWED8wf84cWtd4Kb8oi9AQqX2TcDfM7LfY6f45NXuBHElNCR8om1RHVFPZDUd00GQ5BZs6U')

export default function CheckoutWrapper() {
    const dispatch = useAppDispatch();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        agent.Payments.CreatePaymentIntent()
            .then(basket => dispatch(setBasket(basket)))
             .catch(error => console.log(error))
            .finally(() => setLoading(false))
    }, [dispatch]);

    if (loading) return <LoadingComponent message='Loading checkout...' />

    return (
          <Elements stripe={stripePromise}>
              <CheckoutPage />
          </Elements>
    )
 }