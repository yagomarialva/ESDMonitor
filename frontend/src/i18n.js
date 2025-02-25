import i18n from "i18next";
import { useTranslation, initReactI18next } from "react-i18next";
import ptJSON from './locate/pt.json'
i18n.use(initReactI18next).init({
  resources: {
    pt: { ...ptJSON },
  },
  lng: "pt",
});