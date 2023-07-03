using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    private InterstitialAd intersticial;
    private RewardedAd recompensado;
    public quiz_game qg;
    public bool modoPrueba = true; // Variable para indicar el modo (true = prueba, false = real)
    private string idTextInstersticial, idTextBonificado, bannerID;
    public AudioListener audioListener;

    [Obsolete]
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        CargarIntersticial();
        CargaRecompensado();
        RequestBanner();
    }

    // Instersticial
    [System.Obsolete]
    void CargarIntersticial()
    {
        if (modoPrueba)
        {
            idTextInstersticial = "ca-app-pub-3940256099942544/1033173712"; // ID de prueba
        }
        else
        {
            idTextInstersticial = "ca-app-pub-8702179348190729/7876894517"; // ID real
        }
        this.intersticial = new InterstitialAd(idTextInstersticial);

        // Called when an ad request has successfully loaded.
        this.intersticial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad is shown.
        this.intersticial.OnAdOpening += HandleOnAdOpening;
        // Called when the ad is closed.
        this.intersticial.OnAdClosed += HandleOnAdClosed;


        AdRequest request = new AdRequest.Builder().Build();
        this.intersticial.LoadAd(request);
    }

    //Instersticial Bonificado (Videos)
    [System.Obsolete]
    void CargaRecompensado()
    {
        if (modoPrueba)
        {
            idTextBonificado = "ca-app-pub-3940256099942544/5224354917"; // ID de prueba
        }
        else
        {
            idTextBonificado = "ca-app-pub-8702179348190729/6293081961"; // ID real
        }
        this.recompensado = new RewardedAd(idTextBonificado);

        // Called when an ad request has successfully loaded.
        this.recompensado.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad is shown.
        this.recompensado.OnAdOpening += HandleRewardedAdOpening;
        // Called when the user should be rewarded for interacting with the ad.
        this.recompensado.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.recompensado.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.recompensado.LoadAd(request);
    }

    [Obsolete]
    public void LlamarInterstical()
    {
        if(this.intersticial.IsLoaded())
        {
            this.intersticial.Show();
        }
    }

    [Obsolete]
    public void LlamarRecompensado()
    {
        if(this.recompensado.IsLoaded())
        {
            this.recompensado.Show();
        }
    }


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Intersticial Cargado");
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        //Desactivar musica
        audioListener.gameObject.SetActive(false);
    }

    [Obsolete]
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        CargarIntersticial();
        //activar musica
        audioListener.gameObject.SetActive(true);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Recompensa Cargado");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //Desactivar musica
        audioListener.gameObject.SetActive(false);
    }

    [Obsolete]
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        CargaRecompensado();
        //activar musica
        audioListener.gameObject.SetActive(true);
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        qg.UpdateVida(5);
        qg.ShowAdvertencia("Haz obtenido 3 vidas", true);
    }

    //Banner
    BannerView bannerView;
    void RequestBanner()
    {
        if (modoPrueba)
        {
            bannerID = "ca-app-pub-3940256099942544/6300978111"; // ID de prueba
        }
        else
        {
            bannerID = "ca-app-pub-8702179348190729/2363470115"; // ID real
        }
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
}
